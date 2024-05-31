using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Data.SQLite;
using System.Reflection.Emit;
using System.Windows.Controls.Primitives;


namespace FrankuGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    class Point {
        public int X;
        public int Y;
        public Point(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

    class ConvexHull
    {
    public static double cross(Point O, Point A, Point B)
    {
        return (A.X - O.X) * (B.Y - O.Y) - (A.Y - O.Y) * (B.X - O.X);
    }

    public static List<Point> GetConvexHull(List<Point> points)
    {
        if (points == null)
            return null;

        if (points.Count() <= 1)
            return points;

        int n = points.Count(), k = 0;
        List<Point> H = new List<Point>(new Point[2 * n]);

        points.Sort((a, b) =>
             a.X == b.X ? a.Y.CompareTo(b.Y) : a.X.CompareTo(b.X));

        // Build lower hull
        for (int i = 0; i < n; ++i)
        {
            while (k >= 2 && cross(H[k - 2], H[k - 1], points[i]) <= 0)
                k--;
            H[k++] = points[i];
        }

        // Build upper hull
        for (int i = n - 2, t = k + 1; i >= 0; i--)
        {
            while (k >= t && cross(H[k - 2], H[k - 1], points[i]) <= 0)
                k--;
            H[k++] = points[i];
        }

        return H.Take(k - 1).ToList();
    }

    public static double GetArea(List<Point> points){
        int i,j;
        double area = 0; 

        for (i=0; i < points.Count; i++) {
            j = (i + 1) % points.Count;

            area += points[i].X * points[j].Y;
            area -= points[i].Y * points[j].X;
        }

        area /= 2;
        return (area < 0 ? -area : area);
    }

    }
    public partial class MainWindow : Window
    {
        private SQLiteConnection connection;
        private Boolean isBM;
        private Bitmap currentBitmapFile;
        private static Object _lock = new Object();
        private static int biggestStatic = -1;
        private static string ansStatic = "";
        private static bool segitunya = false;
        private static int MAX_THREAD = 8;
        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabase();
            ReadSQL();
        }

        private void InitializeDatabase()
        {
            connection = new SQLiteConnection("Data Source=biodata.db;Version=3;");
            connection.Open();
        }

        private void ReadSQL()
        {
            string sqlFilePath = "tubes3_stima24.sql";
            if (!File.Exists(sqlFilePath))
            {
                MessageBox.Show("SQL file not found.");
                return;
            }

            string sqlCommands = File.ReadAllText(sqlFilePath);
            string[] commands = sqlCommands.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string commandText in commands)
            {
                Console.WriteLine($"Executing SQL command: {commandText}"); // Log the command text
                try
                {
                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing SQL command: {ex.Message}");
                }
            }

        }


        private void ButtonSelectImageHandle(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap Files|*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bit = new BitmapImage(new Uri(openFileDialog.FileName));
                currentBitmapFile = BitmapImage2Bitmap(bit);
                ImageContainerInput.Source = bit;
                TextBoxSelectedFile.Text = System.IO.Path.GetFileName(openFileDialog.FileName);
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            isBM = !isBM;
        }

        private double handleArea(List<string> queryString, List<string> baseString){
            List<Point> badPoints = new List<Point>();
            for(int i = 0; i < queryString.Count; i++){
                for(int j = 0; j < queryString[i].Length; j++){
                    if(queryString[i][j] != baseString[i][j]){
                        badPoints.Add(new Point(i, j));
                    }
                }
            }
            //MessageBox.Show("HERE");
            List<Point> convexHull = ConvexHull.GetConvexHull(badPoints);
            int totalArea = queryString.Count * queryString[0].Length;
            double area = totalArea - ConvexHull.GetArea(convexHull);
            double percentage = area / totalArea;
            return percentage;
        }

        public void processThread(int i, int j, List<string> pathList, List<string> nameList, List<string> purePath, List<string> queryString) {
            for(; i < j; i++){
                Bitmap baseBitmap = new Bitmap(pathList[i]);
                List<string> baseList = BmpToBinaryString(baseBitmap);
                double notNice = handleArea(queryString, baseList);
                int real = (int)Math.Floor(notNice * 100);
                lock(_lock){
                    if(real > biggestStatic){
                        biggestStatic = real;
                        ansStatic = purePath[i];
                    }
                }
            }
        }

        public Thread StartTheThread(int i, int j, List<string> pathList, List<string> nameList, List<string> purePath, List<string> queryString) {
            var t = new Thread(() => processThread(i, j, pathList, nameList, purePath, queryString));
            t.Start();
            return t;
        }



        private (string, int) findImage(List<string> binaryString) {
            string query = "SELECT * " +
                "FROM sidik_jari; ";
            int biggest = -1;
            string ans = "";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    List<string> pathList = new List<string>();
                    List<string> nameList = new List<string>();
                    List<string> purePath = new List<string>();
                    while(reader.Read()){
                        pathList.Add("../" + reader.GetString(0));
                        purePath.Add(reader.GetString(0));
                        nameList.Add(reader.GetString(1));
                    }
                    int len = purePath.Count;
                    int cur_index = 0;
                    biggestStatic = -1;
                    ansStatic = "";
                    int UNPROCESS_THREADS = MAX_THREAD;
                    int last_starting_pos = 0;
                    List<Thread> active_threads = new List<Thread>();
                    for(int i = 0; i < MAX_THREAD; i++){
                        double takeLenDouble = len / UNPROCESS_THREADS;
                        int takeLen = (int)Math.Floor(takeLenDouble);
                        Thread t = StartTheThread(last_starting_pos, last_starting_pos + takeLen - 1, pathList, nameList, purePath, binaryString);
                        active_threads.Add(t);
                        len -= takeLen;
                        last_starting_pos += takeLen;
                        UNPROCESS_THREADS--;
                    }
                    for(int i = 0; i < MAX_THREAD; i++){
                        active_threads[i].Join();
                    }
                    biggest = biggestStatic;
                    ans = ansStatic;
                }
            }
            return (ans, biggest);
        }

        private List<int> computeLPSArray(string pat, int M) {
            int len = 0;
            int i = 1;
            List<int> lps = new List<int>(M);
            for(int i = 0; i < M; i++) lps.Add(0);
            while(i < M) {
                if(pat[i] == pat[len]) {
                    len++;
                    lps[i] = len;
                    i++;
                } else {
                    if(len != 0){
                        len = lps[len - 1];
                    }else{
                        lps[i] = len;
                        i++
                    }
                }
            }
            return lps;
        }

        private bool KMPSearch(string pat, string txt) {
            int M = pat.Length;
            int N = txt.Length;

            List<int> lps = computeLPSArray(pat, M);
            int i = 0, j = 0;
            while(i < N) {
                if(pat[j] == txt[i]) {
                    j++;
                    i++;
                }
                if(j == M){
                    return true;
                } else if (i < N && pat[j] != txt[i]) {
                    if(j != 0) j = lps[j - 1];
                    else i++;
                }
            } 
            return false;
        }

        private void FindExactMatchKMP(List<string> queryString){
            
        }

        private void FindExactMatchBM(List<string> queryString){

        }

        private void SearchButtonClickHandle(object sender, EventArgs e)
        {
            if (currentBitmapFile != null)
            {
                Bitmap bitmap = currentBitmapFile;

                // start time
                
                // convert to binary
                List<string> binaryString = BmpToBinaryString(bitmap);
                string pathRes = "";
                int similarityRes = 0;
                // convert to ascii
                //string asciiString = BinaryToAscii(binaryString);
                //Console.WriteLine(asciiString);
                Stopwatch stopwatch = Stopwatch.StartNew();
                //MessageBox.Show(algoResult.Item1);
                // implement algorithm
                if (isBM)
                {
                    FindExactMatchBM();
                }
                else
                {
                    FindExactMatchKMP();
                }

                if (!segitunya) {
                    var algoResult = findImage(binaryString);
                    pathRes = algoResult.Item1;
                    similarityRes = algoResult.Item2;
                }

                // stop time
                stopwatch.Stop();


                // RESULT SECTION

                // count time execution
                long executionTimeMs = stopwatch.ElapsedMilliseconds;
                string executionTime = executionTimeMs.ToString();
                TextBoxRuntime.Text = $"Runtime : {executionTimeMs}ms";

                // count percentage
                string percentage = "97";
                TextBoxSimilarityResult.Text = $"Similarity : {similarityRes}%";

                // take data from database
                RetrieveData("Jn Smth"); // example

                // take image from database
                RetrieveImage(pathRes); // example
            }
            else
            {
                MessageBox.Show("Please upload an image first.");
            }
        }

        private void RetrieveData(string name)
        {
            string query = "SELECT * " +
                "FROM biodata " +
                $"WHERE nama = '{name}' " +
                "LIMIT 1;";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string nameResult = reader["nama"].ToString();
                        string tempatLahirResult = reader["tempat_lahir"].ToString();
                        string tanggalLahirResult = ((DateTime)reader["tanggal_lahir"]).ToString("dd-MM-yyyy");
                        string jenisKelaminResult = reader["jenis_kelamin"].ToString();
                        string golonganDarahResult = reader["golongan_darah"].ToString();
                        string alamatResult = reader["alamat"].ToString();
                        string agamaResult = reader["agama"].ToString();
                        string statusPerkawinanResult = reader["status_perkawinan"].ToString();
                        string pekerjaanResult = reader["pekerjaan"].ToString();
                        string kewarganegaraanResult = reader["kewarganegaraan"].ToString();
                        string NIKResult = reader["NIK"].ToString();

                        LabelNama.Content = $"Nama : {nameResult}";
                        LabelNIK.Content = $"NIK : {NIKResult}";
                        LabelTmpLahir.Content = $"Tempat Lahir : {tempatLahirResult}";
                        LabelTglLahir.Content = $"Tanggal Lahir : {tanggalLahirResult}";
                        LabelJenisKelamin.Content = $"Jenis Kelamin : {jenisKelaminResult}";
                        LabelGoldar.Content = $"Golongan Darah : {golonganDarahResult}";
                        LabelAlamatContent.Content = $"{alamatResult}";
                        LabelAgama.Content = $"Agama : {agamaResult}";
                        LabelStatusPerkawinan.Content = $"Status Perkawinan : {statusPerkawinanResult}";
                        LabelPekerjaan.Content = $"Pekerjaan : {pekerjaanResult}";
                        LabelKewarganegaraan.Content = $"Kewarganegaraan : {kewarganegaraanResult}";

                        // Additional processing if needed
                    }
                    else
                    {
                        MessageBox.Show("No data found in the database.");
                    }
                }
            }
        }

        private void RetrieveImage(string name)
        {
            // query here
            string relativePath = "../../../../" + name;
            string absolutePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));

            if (System.IO.File.Exists(absolutePath))
            {
                BitmapImage bit = new BitmapImage(new Uri(absolutePath));
                //pictureBox2.Image = new Bitmap(resultImagePath);
                ImageContainerMatched.Source = bit;
            }
            else
            {
                MessageBox.Show("Result image file not found.");
            }
        }

        private List<string> BmpToBinaryString(Bitmap bmp)
        {
            int threshold = 128;
            List<string> result = new List<string>();
            for (int y = 0; y < bmp.Height; y++)
            {
                StringBuilder binaryString = new StringBuilder();
                for (int x = 0; x < bmp.Width; x++)
                {
                    System.Drawing.Color pixelColor = bmp.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.299 + pixelColor.G * 0.587 + pixelColor.B * 0.114);
                    binaryString.Append(grayValue < threshold ? '1' : '0');
                }
                result.Add(binaryString.ToString());
            }

            return result;
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }

        private string BinaryToAscii(string binaryString)
        {
            StringBuilder asciiString = new StringBuilder();
            StringBuilder currentByte = new StringBuilder();

            foreach (char c in binaryString)
            {
                if (c == '\n')
                {
                    if (currentByte.Length > 0)
                    {
                        while (currentByte.Length < 8)
                        {
                            currentByte.Append('0');
                        }
                        int decimalValue = Convert.ToInt32(currentByte.ToString(), 2);
                        asciiString.Append((char)decimalValue);
                        currentByte.Clear();
                    }
                    asciiString.Append('\n');
                }
                else
                {
                    currentByte.Append(c);
                    if (currentByte.Length == 8)
                    {
                        int decimalValue = Convert.ToInt32(currentByte.ToString(), 2);
                        asciiString.Append((char)decimalValue);
                        currentByte.Clear();
                    }
                }
            }

            if (currentByte.Length > 0)
            {
                while (currentByte.Length < 8)
                {
                    currentByte.Append('0');
                }
                int decimalValue = Convert.ToInt32(currentByte.ToString(), 2);
                asciiString.Append((char)decimalValue);
            }

            return asciiString.ToString();
        }

    }
}
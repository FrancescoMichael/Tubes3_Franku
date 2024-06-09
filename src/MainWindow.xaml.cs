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

using System.Windows.Interop;


namespace FrankuGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private SQLiteConnection connection;
        private Boolean isBM;
        private Bitmap? currentBitmapFile;
        private static Object _lock = new Object();
        private static int biggestStatic = -1;
        private static string ansStatic = "";
        private static bool segitunya = false;
        private static int MAX_THREAD = 18;
        private static int numArea = -1;
        private static List<List<Point>>? resConvexHulls;

        private static string nameString = "";

        private static readonly int MINIMUM_FOUND_PERCENTAGE = 51;

        private static string EncryptionKey = "1234567890abcdef";

        private readonly string connectionString = "Data Source=biodata.db;Version=3;";

        public MainWindow()
        {
            InitializeComponent();
            // InitializeDatabase();
            connection = new SQLiteConnection(connectionString);
            connection.Open();
            ReadSQL();
            EncryptSQL();
            FFT.initFFT();
            List<long> a1 = new List<long>{2, 3};
            List<long> a2 = new List<long>{1, 1};
            List<long> res = FFT.mul(ref a1, ref a2);
            string resString = res[0].ToString() + " " + res[1].ToString() + " " + res[2].ToString();
            //MessageBox.Show(resString);
            currentBitmapFile = null;
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

        private void EncryptSQL() 
        {
            try 
            {
                string selectQuery = "SELECT * FROM biodata;";
                
                using (var selectCommand = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nik = reader["NIK"].ToString()!;
                            string nama = reader["nama"].ToString()!;
                            string tempat_lahir = reader["tempat_lahir"].ToString()!;
                            string tanggal_lahir = reader["tanggal_lahir"].ToString()!;
                            string jenis_kelamin = reader["jenis_kelamin"].ToString()!;
                            string golongan_darah = reader["golongan_darah"].ToString()!;
                            string alamat = reader["alamat"].ToString()!;
                            string agama = reader["agama"].ToString()!;
                            string pekerjaan = reader["pekerjaan"].ToString()!;
                            string kewarganegaraan = reader["kewarganegaraan"].ToString()!;
                            string status_perkawinan = reader["status_perkawinan"].ToString()!;
                            
                            nama = EncryptionTEA.EncryptString(nama, EncryptionKey);
                            tempat_lahir = EncryptionTEA.EncryptString(tempat_lahir, EncryptionKey);
                            tanggal_lahir = EncryptionTEA.EncryptString(tanggal_lahir, EncryptionKey);
                            jenis_kelamin = EncryptionTEA.EncryptString(jenis_kelamin, EncryptionKey);
                            golongan_darah = EncryptionTEA.EncryptString(golongan_darah, EncryptionKey);
                            alamat = EncryptionTEA.EncryptString(alamat, EncryptionKey);
                            agama = EncryptionTEA.EncryptString(agama, EncryptionKey);
                            pekerjaan = EncryptionTEA.EncryptString(pekerjaan, EncryptionKey);
                            kewarganegaraan = EncryptionTEA.EncryptString(kewarganegaraan, EncryptionKey);
                            status_perkawinan = EncryptionTEA.EncryptString(status_perkawinan, EncryptionKey);
                            
                            string updateQuery = $"UPDATE biodata SET " +
                                                $"nama = '{nama}', " +
                                                $"tempat_lahir = '{tempat_lahir}', " +
                                                $"tanggal_lahir = '{tanggal_lahir}', " +
                                                $"jenis_kelamin = '{jenis_kelamin}', " +
                                                $"golongan_darah = '{golongan_darah}', " +
                                                $"alamat = '{alamat}', " +
                                                $"agama = '{agama}', " +
                                                $"status_perkawinan = '{status_perkawinan}', " +
                                                $"pekerjaan = '{pekerjaan}', " +
                                                $"kewarganegaraan = '{kewarganegaraan}' " +
                                                $"WHERE NIK = '{nik}';";
                                                
                            // Execute the UPDATE query
                            using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                            {
                                updateCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show($"Error encrypting Biodata: {ex.Message}");
            }

            try 
            {
                string selectQuery = "SELECT * FROM sidik_jari;";
                
                using (var selectCommand = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string berkas_citra = reader["berkas_citra"].ToString()!;
                            string nama = reader["nama"].ToString()!;
                            
                            nama = EncryptionTEA.EncryptString(nama, EncryptionKey);
                            
                            string updateQuery = $"UPDATE sidik_jari SET " +
                                                $"nama = '{nama}' " +
                                                $"WHERE berkas_citra = '{berkas_citra}';";
                                                
                            using (var updateCommand = new SQLiteCommand(updateQuery, connection))
                            {
                                updateCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show($"Error encrypting Sidik Jari: {ex.Message}");
            }
        }

        private void ButtonSelectImageHandle(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap Files|*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bit = new BitmapImage(new Uri(openFileDialog.FileName));
                currentBitmapFile = Converter.BitmapImage2Bitmap(bit);
                ImageContainerInput.Source = bit;
                TextBoxSelectedFile.Text = System.IO.Path.GetFileName(openFileDialog.FileName);
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            isBM = !isBM;
        }

        private (double, int, List<List<Point>>) handleArea(List<string> queryString, List<string> baseString){
            var startInd = FFT.FindStart(queryString, baseString);
            if(startInd.Item1 == -1 || startInd.Item2 == -1){
                return (0.0, -1, new List<List<Point>>());
            }
            int rowS = startInd.Item1;
            int colS = startInd.Item2;
            List<Point> badPoints = new List<Point>();
            List<List<int>> indexList = new List<List<int>>();
            int cnt = 0;
            List<string> tempBase = new List<string>();
            for(int i = rowS; i < rowS + queryString.Count; i++){
                StringBuilder asciiString = new StringBuilder();
                for(int j = colS; j < colS + queryString[0].Length; j++){
                    asciiString.Append(baseString[i][j]);
                }
                tempBase.Add(asciiString.ToString());
            }
            baseString = tempBase;
            for(int i = 0; i < queryString.Count; i++){
                List<int> indexListTemp = new List<int>();
                for(int j = 0; j < queryString[i].Length; j++){
                    if(queryString[i][j] != baseString[i][j]){
                        //cnt++;
                        badPoints.Add(new Point(i, j));
                        indexListTemp.Add(cnt);
                        cnt++;
                    }else{
                        indexListTemp.Add(-1);
                    }
                }
                indexList.Add(indexListTemp);
            }
            //MessageBox.Show("PROCESS DONE");
            DBSCAN dbs = new DBSCAN(4, 3);
            dbs.changeProp(baseString, queryString, indexList);
            List<int> clusterPoint = dbs.Cluster(badPoints);
            //MessageBox.Show("DBS DONE");
            /*int totalArea = queryString.Count * queryString[0].Length;
            double area = totalArea - cnt;
            double percentage = area / totalArea;
            return percentage;*/
            //MessageBox.Show("HERE");
            List<Point> realBadPoints = new List<Point>();
            int maxi = -1;
            for(int i = 0; i < clusterPoint.Count; i++){
                maxi = Math.Max(maxi, clusterPoint[i]);
            }
            List<List<Point>> listOfPoints = new List<List<Point>>();
            double badArea = 0;
            for(int i = 0; i <= maxi; i++){
                listOfPoints.Add(new List<Point>());
            }
            for(int i = 0; i < clusterPoint.Count; i++){
                if(clusterPoint[i] == -1){
                    badArea += 0.5;
                }else if(clusterPoint[i] != -2){
                    listOfPoints[clusterPoint[i]].Add(badPoints[i]);
                }
            }
            List<List<Point>> tempConvexHulls = new List<List<Point>>();
            for(int i = 0; i <= maxi; i++){
                List<Point> convexHull = ConvexHull.GetConvexHull(listOfPoints[i]);
                badArea += ConvexHull.GetArea(convexHull);
                for(int j = 0; j < convexHull.Count; j++){
                    convexHull[j].X += rowS;
                    convexHull[j].Y += colS;
                }
                tempConvexHulls.Add(convexHull);
            }
            int totalArea = queryString.Count * queryString[0].Length;
            double area = totalArea - badArea;
            double percentage = area / totalArea;
            return (percentage, maxi, tempConvexHulls);
        }

        public void processThread(int i, int j, List<string> pathList, List<string> nameList, List<string> purePath, List<string> queryString) {
            for(; i < j; i++){
                Bitmap baseBitmap = new Bitmap(pathList[i]);
                List<string> baseList = Converter.BmpToBinaryString(baseBitmap);
                //if(baseList.Count != queryString.Count || baseList[0].Length != queryString[0].Length){
                  //  continue;
                //}
                var notNice = handleArea(queryString, baseList);
                int real = (int)Math.Floor(notNice.Item1 * 100);
                int numAreaTemp = notNice.Item2;
                lock(_lock){
                    if(real > biggestStatic){
                        biggestStatic = real;
                        ansStatic = purePath[i];
                        numArea = numAreaTemp;
                        resConvexHulls = notNice.Item3;
                        nameString = nameList[i];
                    }
                }
            }
        }

        public void processKMP(int i, int j, List<string> pathList, List<string> nameList, List<string> purePath, string pattern) {
            for(; i < j && !segitunya; i++){
                Bitmap baseBitmap = new Bitmap(pathList[i]);
                List<string> baseList = Converter.BmpToBinaryString(baseBitmap);
                int firstQuarter = baseList.Count / 4;
                int lastQuarter = baseList.Count - firstQuarter;
                string text = "";
                for(; firstQuarter < lastQuarter; firstQuarter++){
                    text += baseList[firstQuarter];
                }
                bool take = KMP.KMPSearch(pattern, text);
                lock(_lock){
                    if(take){
                        segitunya = true;
                        biggestStatic = 100;
                        ansStatic = purePath[i];
                        nameString = nameList[i];
                        break;
                    }
                }
            }
        }

        public void processBM(int i, int j, List<string> pathList, List<string> nameList, List<string> purePath, string pattern) {
            for(; i < j && !segitunya; i++){
                Bitmap baseBitmap = new Bitmap(pathList[i]);
                List<string> baseList = Converter.BmpToBinaryString(baseBitmap);
                int threeEights = baseList.Count / 20;
                threeEights *= 9;
                int lastThreeEights = baseList.Count - threeEights;
                string text = "";
                for(; threeEights < lastThreeEights; threeEights++){
                    text += baseList[threeEights];
                }
                bool take = BM.BMSearch(pattern, text);
                lock(_lock){
                    if(take){
                        segitunya = true;
                        biggestStatic = 100;
                        ansStatic = purePath[i];
                        nameString = nameList[i];
                        break;
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
                //MessageBox.Show("HERE");
                using (var reader = command.ExecuteReader())
                {
                    List<string> pathList = new List<string>();
                    List<string> nameList = new List<string>();
                    List<string> purePath = new List<string>();
                    while(reader.Read()){
                        pathList.Add("../" + reader.GetString(0));
                        purePath.Add(reader.GetString(0));
                        nameList.Add(EncryptionTEA.DecryptString(reader.GetString(1), EncryptionKey));
                    }
                    int len = purePath.Count;
                    biggestStatic = -1;
                    ansStatic = "";
                    int UNPROCESS_THREADS = MAX_THREAD;
                    int last_starting_pos = 0;
                    List<Thread> active_threads = new List<Thread>();
                    //MessageBox.Show(pathList.Count.ToString());
                    for(int i = 0; i < MAX_THREAD; i++){
                        double takeLenDouble = len / UNPROCESS_THREADS;
                        int takeLen = (int)Math.Floor(takeLenDouble);
                        Thread t = StartTheThread(last_starting_pos, last_starting_pos + takeLen, pathList, nameList, purePath, binaryString);
                        active_threads.Add(t);
                        len -= takeLen;
                        last_starting_pos += takeLen;
                        UNPROCESS_THREADS--;
                    }
                    //MessageBox.Show("YK");
                    for(int i = 0; i < MAX_THREAD; i++){
                        active_threads[i].Join();
                    }
                    biggest = biggestStatic;
                    ans = ansStatic;
                }
            }
            //MessageBox.Show("UYK");
            return (ans, biggest);
        }

        private Thread StartKMPThread(int i, int j, List<string> pathList, List<string> nameList, List<string> purePath, string pattern){
            var t = new Thread(() => processKMP(i, j, pathList, nameList, purePath, pattern));
            t.Start();
            return t;
        }

        private (string, int) FindExactMatchKMP(List<string> queryString){
            string query = "SELECT * " +
                "FROM sidik_jari; ";
            int biggest = -1;
            string ans = "";
            string pattern = "";
            try{
                int middleRow = queryString.Count / 2;
                pattern = queryString[middleRow];
            }catch(Exception ){
                return ("", 0);
            }
            using (var command = new SQLiteCommand(query, connection))
            {
                //MessageBox.Show("HERE");
                using (var reader = command.ExecuteReader())
                {
                    List<string> pathList = new List<string>();
                    List<string> nameList = new List<string>();
                    List<string> purePath = new List<string>();
                    while(reader.Read()){
                        pathList.Add("../" + reader.GetString(0));
                        purePath.Add(reader.GetString(0));
                        nameList.Add(EncryptionTEA.DecryptString(reader.GetString(1), EncryptionKey));
                    }
                    int len = purePath.Count;
                    biggestStatic = -1;
                    ansStatic = "";
                    int UNPROCESS_THREADS = MAX_THREAD;
                    int last_starting_pos = 0;
                    List<Thread> active_threads = new List<Thread>();
                    //MessageBox.Show(pathList.Count.ToString());
                    for(int i = 0; i < MAX_THREAD; i++){
                        double takeLenDouble = len / UNPROCESS_THREADS;
                        int takeLen = (int)Math.Floor(takeLenDouble);
                        Thread t = StartKMPThread(last_starting_pos, last_starting_pos + takeLen, pathList, nameList, purePath, pattern);
                        active_threads.Add(t);
                        len -= takeLen;
                        last_starting_pos += takeLen;
                        UNPROCESS_THREADS--;
                    }
                    //MessageBox.Show("YK");
                    for(int i = 0; i < MAX_THREAD; i++){
                        active_threads[i].Join();
                    }
                    biggest = biggestStatic;
                    ans = ansStatic;
                }
            }
            return (ans, biggest);
        }

        private Thread StartBMThread(int i, int j, List<string> pathList, List<string> nameList, List<string> purePath, string pattern){
            var t = new Thread(() => processBM(i, j, pathList, nameList, purePath, pattern));
            t.Start();
            return t;
        }

        private (string, int) FindExactMatchBM(List<string> queryString){
            string query = "SELECT * " +
                "FROM sidik_jari; ";
            int biggest = -1;
            string ans = "";
            string pattern = "";
            try{
                int middleRow = queryString.Count / 2;
                pattern = queryString[middleRow];
            }catch(Exception ){
                return ("", 0);
            }
            using (var command = new SQLiteCommand(query, connection))
            {
                //MessageBox.Show("HERE");
                using (var reader = command.ExecuteReader())
                {
                    List<string> pathList = new List<string>();
                    List<string> nameList = new List<string>();
                    List<string> purePath = new List<string>();
                    while(reader.Read()){
                        pathList.Add("../" + reader.GetString(0));
                        purePath.Add(reader.GetString(0));
                        nameList.Add(EncryptionTEA.DecryptString(reader.GetString(1), EncryptionKey));
                    }
                    int len = purePath.Count;
                    biggestStatic = -1;
                    ansStatic = "";
                    int UNPROCESS_THREADS = MAX_THREAD;
                    int last_starting_pos = 0;
                    List<Thread> active_threads = new List<Thread>();
                    //MessageBox.Show(pathList.Count.ToString());
                    for(int i = 0; i < MAX_THREAD; i++){
                        double takeLenDouble = len / UNPROCESS_THREADS;
                        int takeLen = (int)Math.Floor(takeLenDouble);
                        Thread t = StartBMThread(last_starting_pos, last_starting_pos + takeLen, pathList, nameList, purePath, pattern);
                        active_threads.Add(t);
                        len -= takeLen;
                        last_starting_pos += takeLen;
                        UNPROCESS_THREADS--;
                    }
                    //MessageBox.Show("YK");
                    for(int i = 0; i < MAX_THREAD; i++){
                        active_threads[i].Join();
                    }
                    biggest = biggestStatic;
                    ans = ansStatic;
                }
            }
            return (ans, biggest);
        }

        private async void SearchButtonClickHandle(object sender, EventArgs e)
        {
            ButtonSearch.IsEnabled = false;
            ToggleAlgorithm.IsEnabled = false;
            ButtonSelectImage.IsEnabled = false;
            LabelLoading.Visibility = Visibility.Visible;
            EllipseLoadingIndicator.Visibility = Visibility.Visible;
            if (currentBitmapFile != null)
            {
                int similarityRes = 0;
                long executionTimeMs = 0;
                string pathRes = "";

                await Task.Run(() => {
                segitunya = false;
                biggestStatic = -1;
                ansStatic = "";
                numArea = -1;
                resConvexHulls = null;
                nameString = "";
                Bitmap bitmap = currentBitmapFile;

                // start time
                
                // convert to binary
                List<string> binaryString = Converter.BmpToBinaryString(bitmap);
                // convert to ascii
                //string asciiString = BinaryToAscii(binaryString);
                //Console.WriteLine(asciiString);
                Stopwatch stopwatch = Stopwatch.StartNew();
                //MessageBox.Show(algoResult.Item1);
                // implement algorithm
                if (isBM)
                {
                    var algoResult = FindExactMatchBM(binaryString);
                    pathRes = algoResult.Item1;
                    similarityRes = algoResult.Item2;
                }
                else
                {
                    var algoResult =  FindExactMatchKMP(binaryString);
                    pathRes = algoResult.Item1;
                    similarityRes = algoResult.Item2;
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
                executionTimeMs = stopwatch.ElapsedMilliseconds;

                });

                if(biggestStatic < MINIMUM_FOUND_PERCENTAGE){
                    LabelLoading.Visibility = Visibility.Hidden;
                    EllipseLoadingIndicator.Visibility = Visibility.Hidden;
                    TextBoxSimilarityResult.Text = $"Similarity : -1%";
                    ButtonSearch.IsEnabled = true;
                    ToggleAlgorithm.IsEnabled = true;
                    ButtonSelectImage.IsEnabled = true;
                    // MessageBox.Show(biggestStatic.ToString());
                    LabelNama.Content = $"Nama : ";
                    LabelNIK.Content = $"NIK : ";
                    LabelTmpLahir.Content = $"Tempat Lahir : ";
                    LabelTglLahir.Content = $"Tanggal Lahir : ";
                    LabelJenisKelamin.Content = $"Jenis Kelamin : ";
                    LabelGoldar.Content = $"Golongan Darah : ";
                    LabelAlamatContent.Content = $"";
                    LabelAgama.Content = $"Agama : ";
                    LabelStatusPerkawinan.Content = $"Status Perkawinan : ";
                    LabelPekerjaan.Content = $"Pekerjaan : ";
                    LabelKewarganegaraan.Content = $"Kewarganegaraan : ";
                    ImageContainerMatched.Source = null;
                    RetrieveData("");
                    RetrieveImage("");
                    return;
                }

                string executionTime = executionTimeMs.ToString();
                TextBoxRuntime.Text = $"Runtime : {executionTime}ms";

                // count percentage
                TextBoxSimilarityResult.Text = $"Similarity : {biggestStatic}%";

                //MessageBox.Show("DONEALL");

                // take data from database
                FindNameAndRetrieveData(); // example

                // take image from database
                RetrieveImage(pathRes); // example
            }
            else
            {
                MessageBox.Show("Please upload an image first.");
            }
            ButtonSearch.IsEnabled = true;
            ToggleAlgorithm.IsEnabled = true;
            ButtonSelectImage.IsEnabled = true;
            LabelLoading.Visibility = Visibility.Hidden;
            EllipseLoadingIndicator.Visibility = Visibility.Hidden;
        }

        private void FindNameAndRetrieveData(){
            string query = "SELECT * " +
                "FROM biodata; ";
            bool fd = false;
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    List<string> names = new List<string>();
                    while(reader.Read()){
                        names.Add(EncryptionTEA.DecryptString(reader["nama"].ToString()!, EncryptionKey));
                    }
                    for(int i = 0; i < names.Count && !fd; i++){
                        if(RegexSpec.regexMatch(nameString, names[i])){
                            RetrieveData(names[i]);
                            fd = true;
                        }
                    }
                    if(!fd){
                        RetrieveData("!!!");
                    }
                }
            }
        }

        private void RetrieveData(string name)
        {
            // MessageBox.Show(name);
            string encryptedName = EncryptionTEA.EncryptString(name, EncryptionKey);
            // MessageBox.Show(encryptedName);
            string query = "SELECT * " +
                "FROM biodata " +
                $"WHERE nama = '{encryptedName}' " +
                "LIMIT 1;";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    
                    if (reader.Read())
                    {   
                        string NIKResult = reader["NIK"].ToString()!;
                        string nameResult = nameString;
                        string tempatLahirResult = reader["tempat_lahir"].ToString()!;
                        string tanggalLahirResult = reader["tanggal_lahir"].ToString()!;
                        string jenisKelaminResult = reader["jenis_kelamin"].ToString()!;
                        string golonganDarahResult = reader["golongan_darah"].ToString()!;
                        string alamatResult = reader["alamat"].ToString()!;
                        string agamaResult = reader["agama"].ToString()!;
                        string statusPerkawinanResult = reader["status_perkawinan"].ToString()!;
                        string pekerjaanResult = reader["pekerjaan"].ToString()!;
                        string kewarganegaraanResult = reader["kewarganegaraan"].ToString()!;
                        

                        LabelNama.Content = $"Nama : {nameResult}";
                        // LabelNIK.Content = $"NIK : {EncryptionTEA.DecryptString(NIKResult, EncryptionKey)}";
                        LabelNIK.Content = $"NIK : {NIKResult}";
                        LabelTmpLahir.Content = $"Tempat Lahir : {EncryptionTEA.DecryptString(tempatLahirResult, EncryptionKey)}";
                        // LabelTmpLahir.Content = $"Tempat Lahir : {tempatLahirResult}";
                        LabelTglLahir.Content = $"Tanggal Lahir : {EncryptionTEA.DecryptString(tanggalLahirResult, EncryptionKey)}";
                        LabelJenisKelamin.Content = $"Jenis Kelamin : {EncryptionTEA.DecryptString(jenisKelaminResult, EncryptionKey)}";
                        LabelGoldar.Content = $"Golongan Darah : {EncryptionTEA.DecryptString(golonganDarahResult, EncryptionKey)}";
                        LabelAlamatContent.Content = $"{EncryptionTEA.DecryptString(alamatResult, EncryptionKey)}";
                        LabelAgama.Content = $"Agama : {EncryptionTEA.DecryptString(agamaResult, EncryptionKey)}";
                        LabelStatusPerkawinan.Content = $"Status Perkawinan : {EncryptionTEA.DecryptString(statusPerkawinanResult, EncryptionKey)}";
                        LabelPekerjaan.Content = $"Pekerjaan : {EncryptionTEA.DecryptString(pekerjaanResult, EncryptionKey)}";
                        LabelKewarganegaraan.Content = $"Kewarganegaraan : {EncryptionTEA.DecryptString(kewarganegaraanResult, EncryptionKey)}";
                        
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
            //MessageBox.Show("SAMWER");
            string absolutePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));

            if (System.IO.File.Exists(absolutePath) && resConvexHulls != null)
            {
                BitmapImage bit = new BitmapImage();
                var stream = File.OpenRead(absolutePath);
                bit.BeginInit();
                bit.CacheOption = BitmapCacheOption.OnLoad;
                bit.StreamSource = stream;
                bit.EndInit();
                using(Bitmap baseBitmap = Converter.BitmapImage2Bitmap(bit))
                using(Bitmap changedBitmap = new Bitmap(baseBitmap.Width, baseBitmap.Height))
                using(Bitmap finalBitmap = new Bitmap(changedBitmap.Width, changedBitmap.Height)){
                //pictureBox2.Image = new Bitmap(resultImagePath);


                //changedBitmap.MakeTransparent();
                int[,] borderTable = new int[changedBitmap.Height, changedBitmap.Width];
                for(int i = 0; i < resConvexHulls.Count; i++){
                    int maxi = -1, mini = (int)1e5;
                    if(resConvexHulls[i].Count < 3) continue;
                    for(int j = 0; j < resConvexHulls[i].Count; j++){
                        maxi = Math.Max(maxi, resConvexHulls[i][j].X);
                        mini = Math.Min(mini, resConvexHulls[i][j].X);
                    }
                    for(int j = 0; j < resConvexHulls[i].Count; j++){
                        Point curP = resConvexHulls[i][j];
                        int k = (j + 1) % resConvexHulls[i].Count;
                        Point nextP = resConvexHulls[i][k];
                        borderTable[curP.X, curP.Y] = 1;
                        if(curP.Y == nextP.Y){ // vertical
                            int x1S = curP.X;
                            int x2S = nextP.X;
                            int adderS = ((x1S > x2S) ? -1 : 1);
                            x1S += adderS;
                            for(; x1S != x2S; x1S += adderS){
                                borderTable[x1S, curP.Y] = 1;
                            }
                            continue;
                        }
                        if(curP.X == nextP.X){
                            continue;
                        }
                        int x1 = curP.Y;
                        int y1 = curP.X;
                        int x2 = nextP.Y;
                        int y2 = nextP.X;
                        int adder = ((y1 > y2) ? -1 : 1);
                        double m = ((y2 - y1) * 1.0) / ((x2 - x1) * 1.0);
                        double c = (y1 * 1.0) - m * x1;
                        y1 += adder;
                        for(; y1 != y2; y1 += adder){
                            double xUwu = ((y1 * 1.0 - c) / m);
                            int cTemp = (int)Math.Round(xUwu);
                            while(cTemp < changedBitmap.Width && borderTable[y1, cTemp] == 1) cTemp++;
                            if(cTemp < changedBitmap.Width){
                                borderTable[y1, cTemp] = 1;
                            }
                        } 
                    }
                    for(int j = 0; j < resConvexHulls[i].Count; j++){
                        borderTable[resConvexHulls[i][j].X, resConvexHulls[i][j].Y] = 1;
                    }
                    int maxiCnt = 0, miniCnt = 0;
                    int maxiP = -1, miniP = -1;
                    for(int j = 0; j < resConvexHulls[i].Count; j++){
                        if(resConvexHulls[i][j].X == maxi){
                            maxiCnt ^= 1;
                            maxiP = j;
                            string showJ = j.ToString();
                            //MessageBox.Show("YEA" + showJ);
                        }
                        if(resConvexHulls[i][j].X == mini){
                            miniCnt ^= 1;
                            miniP = j;
                        }
                    }
                    if(maxiCnt == 1){
                        borderTable[resConvexHulls[i][maxiP].X, resConvexHulls[i][maxiP].Y] = 2;
                    }
                    if(miniCnt == 1){
                        borderTable[resConvexHulls[i][miniP].X, resConvexHulls[i][miniP].Y] = 2;
                    }
                }
                for(int y = 0; y < changedBitmap.Height; y++){
                    int stat = 0;
                    for(int x = 0; x < changedBitmap.Width; x++){
                        if(borderTable[y, x] != 0 || stat == 1){
                            changedBitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(75, System.Drawing.Color.Red));
                        }
                        if(borderTable[y, x] == 1){
                            stat ^= 1;
                        }
                    }
                }
                using (Graphics g = Graphics.FromImage(finalBitmap))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Black);

                    //go through each image and draw it on the final image (Notice the offset; since I want to overlay the images i won't have any offset between the images in the finalImage)
                    
                    g.DrawImage(baseBitmap, new System.Drawing.Rectangle(0, 0, changedBitmap.Width, changedBitmap.Height));
                    g.DrawImage(changedBitmap, new System.Drawing.Rectangle(0, 0, changedBitmap.Width, changedBitmap.Height));

                }
                bit.Freeze();
                stream.Close();
                stream.Dispose();
                BitmapImage changedImage = Converter.Bitmap2BitmapImage(finalBitmap);
                ImageContainerMatched.Source = changedImage;
                }
            }
            else if(System.IO.File.Exists(absolutePath)){
                BitmapImage bit = new BitmapImage(new Uri(absolutePath));
                ImageContainerMatched.Source = bit;
            }
            else{
                MessageBox.Show("Result image file not found.");
            }
        }
    }
}
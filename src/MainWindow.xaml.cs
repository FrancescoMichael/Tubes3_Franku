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
using System.Text.RegularExpressions;


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
            return new List<Point>();

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

    class DBSCAN
    {
    public int eps;
    public int minPts;
    public List<Point> data;  // supplied in cluster()
    public List<int> labels;  // supplied in cluster()
    public List<string> baseList;
    public List<string> queryList;
    public List<List<int>> indexList;
    public int M;
    public int N;

    public DBSCAN(int eps, int minPts)
    {
      this.eps = eps;
      this.minPts = minPts;
      this.labels = new List<int>();
      this.M = 0;
      this.N = 0;
      this.baseList = new List<string>();
      this.queryList = new List<string>();
      this.indexList = new List<List<int>>();
      this.data = new List<Point>();
      this.labels = new List<int>();
    }  

    public void changeProp(List<string> baseList, List<string> queryList, List<List<int>> indexList){
        this.baseList = baseList;
        this.queryList = queryList;
        this.indexList = indexList;
        this.N = baseList.Count;
        this.M = baseList[0].Length;
    }

    public List<int> Cluster(List<Point> data)
    {
      this.data = data;  // by reference
      
      if(this.labels.Count != this.data.Count){
        this.labels = new List<int>(this.data.Count);
        for(int i = 0; i < this.data.Count; i++){
            this.labels.Add(-2);
        }
      }else{
        for(int i = 0; i < this.data.Count; i++){
            this.labels[i] = -2;
        }
      }
      //this.labels = new int[this.data.Length]; 
      // unprocessed

      int cid = -1;  // offset the start
      for (int i = 0; i < this.data.Count; ++i)
      {
        if (this.labels[i] != -2)  // has been processed
          continue;

        List<int> neighbors = this.RegionQuery(i);
        if (neighbors.Count < this.minPts)
        {
          this.labels[i] = -1;  // noise
        }
        else
        {
          ++cid;
          this.Expand(i, neighbors, cid);
        }
      }

      return this.labels;
    }

    private List<int> RegionQuery(int p)
    {
      List<int> result = new List<int>();
      Point askedPoint = this.data[p];
      int xPoint = askedPoint.X;
      int yPoint = askedPoint.Y;
      for(int i = Math.Max(0, xPoint - eps); i <= Math.Min(N - 1, xPoint + eps); i++){
        for(int j = Math.Max(0, yPoint - eps); j <= Math.Min(M - 1, yPoint + eps); j++){
            int uwu = (i - xPoint) * (i - xPoint) + (j - yPoint) * (j - yPoint);
            if(uwu > eps * eps) continue;
            if(baseList[i][j] != queryList[i][j]){
                result.Add(this.indexList[i][j]);
            }
        }
      }
      return result;
    }

    private void Expand(int p, List<int> neighbors, int cid)
    {
      this.labels[p] = cid;
      //int i = 0;
      //while(i < neighbors.Count)
      for (int i = 0; i < neighbors.Count; ++i)
      {
        int pn = neighbors[i];
        if (this.labels[pn] == -1)  // noise
          this.labels[pn] = cid;
        else if (this.labels[pn] == -2)  // unprocessed
        {
          this.labels[pn] = cid;
          List<int> newNeighbors = this.RegionQuery(pn);
          if (newNeighbors.Count >= this.minPts)
            neighbors.AddRange(newNeighbors); // modifies loop
        }
        //++i;
      }
    }

    private static double EucDistance(double[] x1,
      double[] x2)
    {
      int dim = x1.Length;
      double sum = 0.0;
      for (int i = 0; i < dim; ++i)
        sum += (x1[i] - x2[i]) * (x1[i] - x2[i]);
      return Math.Sqrt(sum);
    }

  } // class DBSCAN

  class FFT{
    private static int LOGN = 18;
    private static List<long>[] iw = new List<long>[LOGN];
    private static List<long>[] rv = new List<long>[LOGN];
    private static List<long>[] w = new List<long>[LOGN];
    private static long mod = 998244353;
    private static long g = 3;
    private static double LOWER_BOUND_TO_CONTINUE = 0.75;

    public static long powMod(long x, long y, long p){
        long res = 1;
        x %= p;
        if(x == 0){
            return 0;
        }
        while(y > 0){
            if((y & 1L) == 1){
                res = ((res % p) * (x % p)) % p;
            }
            y >>= 1;
            x = ((x % p) * (x % p)) % p;
        }
        return res;
    }

    public static long inverseMod(long x, long p){
        return powMod(x, p - 2, p);
    }

    public static void initFFT(){
        long wb = powMod(g, (mod - 1) / (1 << LOGN), mod);
        //MessageBox.Show("DONE POWMOD");
        for(int i = 0; i < LOGN; i++){
            iw[i] = new List<long>();
            rv[i] = new List<long>();
            w[i] = new List<long>();
        }
        for(int st = 0; st < LOGN; st++){
            for(int i = 0; i < (1 << st); i++){
                w[st].Add(1);
                iw[st].Add(1);
                rv[st].Add(0);
            }
            long bw = powMod(wb, 1 << (LOGN - st - 1), mod);
            long ibw = inverseMod(bw, mod);
            long cw = 1;
            long icw = 1;
            for(int k = 0; k < (1 << st); k++){
                w[st][k] = cw;
                iw[st][k] = icw;
                cw = (cw * bw) % mod;
                icw = (icw * ibw) % mod;
            }
            if(st == 0){
                rv[st][0] = 0;
                continue;
            }
            int h = (1 << (st - 1));
            for(int k = 0; k < (1 << st); k++){
                long u = 1;
                if(k < h) u = 0;
                rv[st][k] = (rv[st - 1][k & (h - 1)] << 1) | u;
            }
            //string awa = st.ToString();
            //MessageBox.Show(awa);
        }
    }

    private static void fft(ref List<long> a, bool inverse){
        long n = a.Count;
        long ln = (long)Math.Log2(n * 1.0);
        for(int i = 0; i < n; i++){
            int ni = (int)rv[ln][i];
            if(i < ni){
                long temp = a[i];
                a[i] = a[ni];
                a[ni] = temp;
            }
        }
        //MessageBox.Show("YEEZY TAUGHT U WELL");
        for(int st = 0; (1 << st) < n; st++){
            int len = (1 << st);
            for(int k = 0; k < n; k += (len << 1)){
                for(int pos = k; pos < k + len; pos++){
                    long l = a[pos];
                    long r = (a[pos + len] * (inverse ? iw[st][pos - k] : w[st][pos - k])) % mod;
                    a[pos] = ((l + r < mod) ? l + r : l + r - mod);
                    a[pos + len] = ((l - r >= 0) ? l - r : mod + l - r);
                }
            }
        }
        if(inverse){
            long inValue = inverseMod(n, mod);
            for(int i = 0; i < n; i++){
                a[i] = (a[i] * inValue) % mod;
            }
        }
    }

    public static List<long> mul(ref List<long> a, ref List<long> b){
        List<long> fa = new List<long>(a);
        List<long> fb = new List<long>(b);
        int n = 1;
        while(n < a.Count + b.Count){
            n <<= 1;
        }
        fa.AddRange(Enumerable.Repeat(0L, n - fa.Count));
        fb.AddRange(Enumerable.Repeat(0L, n - fb.Count));
        //MessageBox.Show("REIMPULSTED");
        fft(ref fa, false);
        //MessageBox.Show("DOWNLOAD");
        fft(ref fb, false);
        //MessageBox.Show("H@");
        for(int i = 0; i < n; i++){
            fa[i] = fa[i] * fb[i] % mod;
        }
        fft(ref fa, true);
        //MessageBox.Show("BOOTLEGGER");
        return fa;
    }

    public static (int, int) FindStart(List<string> queryString, List<string> baseString){
        int N1 = queryString.Count;
        int M1 = queryString[0].Length;
        int N2 = baseString.Count;
        int M2 = baseString[0].Length;
        if(N1 > N2 || M1 > M2){
            return (-1, -1);
        }
        if(N1 == N2 && M1 == M2){
            return (0, 0);
        }
        List<long> realBase = new List<long>();
        List<long> invertedBase = new List<long>();
        List<long> realQuery = new List<long>();
        List<long> invertedQuery = new List<long>();
        for(int i = 0; i < N2; i++){
            for(int j = 0; j < M2; j++){
                long val = baseString[i][j] - '0';
                realBase.Add(val);
                invertedBase.Add(val ^ 1L);
            }
        }
        int maxim = N1 * M2 - 1;
        for(int i = 0; i < N1; i++){
            for(int j = 0; j < M1; j++){
                long val = queryString[i][j] - '0';
                realQuery.Add(val);
                invertedQuery.Add(val ^ 1L);
            }
            realQuery.AddRange(Enumerable.Repeat(0L, M2 - M1));
            invertedQuery.AddRange(Enumerable.Repeat(0L, M2 - M1));
        }
        realQuery.Reverse();
        invertedQuery.Reverse();
        if(realQuery.Count != maxim + 1 || invertedQuery.Count != maxim + 1){
            //MessageBox.Show("?????W");
        }
        List<long> res1 = mul(ref realBase, ref realQuery);
        List<long> res2 = mul(ref invertedBase, ref invertedQuery);
        //MessageBox.Show("SOMEONE DONE!!");
        int r1c = res1.Count;
        int r2c = res2.Count;
        int curMaxi = -1, ansI = -1, ansJ = -1;
        for(int i = 0; i <= N2 - N1; i++){
            for(int j = 0; j <= M2 - M1; j++){
                int diff = i * M2 + j;
                diff += maxim;
                int take = 0;
                if(diff < r1c){
                    take += (int)res1[diff];
                }
                if(diff < r2c){
                    take += (int)res2[diff];
                }
                if(take > curMaxi){
                    curMaxi = take;
                    ansI = i;
                    ansJ = j;
                }
            }
        }
        double percentage = 0;
        if(curMaxi != -1){
            percentage = (curMaxi * 1.0) / (N1 * M1 * 1.0);
        }
        if(percentage > 1){
            MessageBox.Show("???PER");
        }
        if(percentage < LOWER_BOUND_TO_CONTINUE){
            return (-1, -1);
        }
        return (ansI, ansJ);
    }

  } // class FFT

    class EncryptionTEA
    {
        private static readonly uint Delta = 0x9e3779b9;
        private static readonly int Rounds = 32;

        public static byte[] Encrypt(byte[] data, byte[] key)
        { 
            uint v0 = BitConverter.ToUInt32(data, 0);
            uint v1 = BitConverter.ToUInt32(data, 4);
            uint k0 = BitConverter.ToUInt32(key, 0);
            uint k1 = BitConverter.ToUInt32(key, 4);
            uint k2 = BitConverter.ToUInt32(key, 8);
            uint k3 = BitConverter.ToUInt32(key, 12);

            uint sum = 0;
            for (int i = 0; i < Rounds; i++)
            {
                sum += Delta;
                v0 += ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
                v1 += ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
            }

            byte[] encrypted = new byte[8];
            Array.Copy(BitConverter.GetBytes(v0), 0, encrypted, 0, 4);
            Array.Copy(BitConverter.GetBytes(v1), 0, encrypted, 4, 4);
            return encrypted;
        }

        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            uint v0 = BitConverter.ToUInt32(data, 0);
            uint v1 = BitConverter.ToUInt32(data, 4);
            uint k0 = BitConverter.ToUInt32(key, 0);
            uint k1 = BitConverter.ToUInt32(key, 4);
            uint k2 = BitConverter.ToUInt32(key, 8);
            uint k3 = BitConverter.ToUInt32(key, 12);

            uint sum = Delta * (uint)Rounds;
            for (int i = 0; i < Rounds; i++)
            {
                v1 -= ((v0 << 4) + k2) ^ (v0 + sum) ^ ((v0 >> 5) + k3);
                v0 -= ((v1 << 4) + k0) ^ (v1 + sum) ^ ((v1 >> 5) + k1);
                sum -= Delta;
            }

            byte[] decrypted = new byte[8];
            Array.Copy(BitConverter.GetBytes(v0), 0, decrypted, 0, 4);
            Array.Copy(BitConverter.GetBytes(v1), 0, decrypted, 4, 4);
            return decrypted;
        }

        public static string EncryptString(string plainText, string key)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            byte[] paddedData = Pad(data);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            byte[] encryptedData = new byte[paddedData.Length];
            for (int i = 0; i < paddedData.Length; i += 8)
            {
                byte[] block = new byte[8];
                Array.Copy(paddedData, i, block, 0, 8);
                byte[] encryptedBlock = Encrypt(block, keyBytes);
                Array.Copy(encryptedBlock, 0, encryptedData, i, 8);
            }

            return Convert.ToBase64String(encryptedData);
        }

        public static string DecryptString(string encryptedText, string key)
        {
            byte[] data = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            byte[] decryptedData = new byte[data.Length];
            for (int i = 0; i < data.Length; i += 8)
            {
                byte[] block = new byte[8];
                Array.Copy(data, i, block, 0, 8);
                byte[] decryptedBlock = Decrypt(block, keyBytes);
                Array.Copy(decryptedBlock, 0, decryptedData, i, 8);
            }

            return Encoding.UTF8.GetString(Unpad(decryptedData));
        }

        private static byte[] Pad(byte[] data)
        {
            int paddingLength = 8 - (data.Length % 8);
            byte[] paddedData = new byte[data.Length + paddingLength];
            Array.Copy(data, 0, paddedData, 0, data.Length);
            for (int i = data.Length; i < paddedData.Length; i++)
            {
                paddedData[i] = (byte)paddingLength;
            }
            return paddedData;
        }

        private static byte[] Unpad(byte[] data)
        {
            int paddingLength = data[data.Length - 1];
            byte[] unpaddedData = new byte[data.Length - paddingLength];
            Array.Copy(data, 0, unpaddedData, 0, unpaddedData.Length);
            return unpaddedData;
        }
    }

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
        private static int ALPHABET_SIZE = 2;
        private static char FIRST_CHARACTER = '0';
        private static int numArea = -1;
        private static List<List<Point>>? resConvexHulls;

        private static string nameString = "";

        private static int MINIMUM_FOUND_PERCENTAGE = 51;

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
                currentBitmapFile = BitmapImage2Bitmap(bit);
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
                List<string> baseList = BmpToBinaryString(baseBitmap);
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
                List<string> baseList = BmpToBinaryString(baseBitmap);
                int firstQuarter = baseList.Count / 4;
                int lastQuarter = baseList.Count - firstQuarter;
                string text = "";
                for(; firstQuarter < lastQuarter; firstQuarter++){
                    text += baseList[firstQuarter];
                }
                bool take = KMPSearch(pattern, text);
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
                List<string> baseList = BmpToBinaryString(baseBitmap);
                int threeEights = baseList.Count / 20;
                threeEights *= 9;
                int lastThreeEights = baseList.Count - threeEights;
                string text = "";
                for(; threeEights < lastThreeEights; threeEights++){
                    text += baseList[threeEights];
                }
                bool take = BMSearch(pattern, text);
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

        private List<int> computeLPSArray(string pat, int M) {
            int len = 0;
            int i = 1;
            List<int> lps = new List<int>(M);
            for(int j = 0; j < M; j++){
                lps.Add(0);
            }
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
                        i++;
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

        private bool BMSearch(string pat, string txt) {
            int M = pat.Length;
            int N = txt.Length;
            List<int> badChar = new List<int>();
            for(int u = 0; u < ALPHABET_SIZE; u++) badChar.Add(-1);
            int i = 0;
            while(i <= N - M) {
                int j = M - 1;
                while(j >= 0 && pat[j] == txt[i + j]){
                    j--;
                }
                if(j < 0){
                    return true;
                }
                i += Math.Max(1, j - badChar[txt[i + j] - FIRST_CHARACTER]);
            } 
            return false;
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
                List<string> binaryString = BmpToBinaryString(bitmap);
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
                    RetrieveData("");
                    RetrieveImage("");
                    TextBoxSimilarityResult.Text = $"Similarity : -1%";
                    ButtonSearch.IsEnabled = true;
                    ToggleAlgorithm.IsEnabled = true;
                    ButtonSelectImage.IsEnabled = true;
                    // MessageBox.Show(biggestStatic.ToString());
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
                        if(regexMatch(nameString, names[i])){
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

        private bool regexMatch(string plainText, string comparedText){
            string regexText = "";

            for (int i = 0; i < plainText.Length; i++)
            {
                char c = plainText[i];

                if (c == 'a' || c == 'A')
                {
                    regexText += "(?:[Aa]|4)?";
                }
                else if (c == 'i' || c == 'I')
                {
                    regexText += "(?:[Ii]|1)?";
                }
                else if (c == 'u' || c == 'U')
                {
                    regexText += "(?:[Uu])?";
                }
                else if (c == 'e' || c == 'E')
                {
                    regexText += "(?:[Ee]|3)?";
                }
                else if (c == 'o' || c == 'O')
                {
                    regexText += "(?:[Oo]|0)?";
                }
                else if (c == 'g' || c == 'G')
                {
                    regexText += "(?:[Gg]|6|9)";
                }
                else if (c == 's' || c == 'S')
                {
                    regexText += "(?:[Ss]|5)";
                }
                else if (c == 'r' || c == 'R')
                {
                    regexText += "(?:[Rr]|12)";
                }
                else if (c == 'b' || c == 'B')
                {
                    regexText += "(?:[Bb]|13|8)";
                }
                else if (c == 't' || c == 'T')
                {
                    regexText += "(?:[Tt]|7)";
                }
                else if (c == 'l' || c == 'L')
                {
                    regexText += "(?:[Ll]|1)";
                }
                else
                {
                    regexText += "(?:[" + char.ToLower(c) + char.ToUpper(c) + "])";
                }
            }
            bool isMatch = Regex.IsMatch(comparedText, regexText);
            return isMatch;
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
                using(Bitmap baseBitmap = BitmapImage2Bitmap(bit))
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
                BitmapImage changedImage = Bitmap2BitmapImage(finalBitmap);
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

        private BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
        private List<string> BmpToBinaryString(Bitmap bmp)
        {
            /*
            int threshold = 128;
            List<string> result = new List<string>();
            for (int y = 0; y < bmp.Height; y++)
            {
                StringBuilder asciiString = new StringBuilder();
                for (int x = 0; x < bmp.Width; x += 8)
                {
                    int res = 0;
                    int u = 0;
                    for(; u < 8 && x + u < bmp.Width; u++){
                        res <<= 1;
                        System.Drawing.Color pixelColor = bmp.GetPixel(x + u, y);
                        int grayValue = (int)(pixelColor.R * 0.299 + pixelColor.G * 0.587 + pixelColor.B * 0.114);
                        res |= (grayValue < threshold ? 1 : 0);                        
                    }
                    for(; u < 8; u++){
                        res <<= 1;
                    }
                    asciiString.Append((char)res);
                }
                result.Add(asciiString.ToString());
            }*/
            int threshold = 128;
            List<string> result = new List<string>();
            for (int y = 0; y < bmp.Height; y++)
            {
                StringBuilder asciiString = new StringBuilder();
                for (int x = 0; x < bmp.Width; x++)
                {
                    System.Drawing.Color pixelColor = bmp.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.299 + pixelColor.G * 0.587 + pixelColor.B * 0.114);
                    asciiString.Append((grayValue < threshold ? '1' : '0'));
                }
                result.Add(asciiString.ToString());
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
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
    public partial class MainWindow : Window
    {
        private SQLiteConnection connection;
        private Boolean isBM;
        private Bitmap currentBitmapFile;
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
            string sqlFilePath = "../../../tubes3_stima24.sql";
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

        private void SearchButtonClickHandle(object sender, EventArgs e)
        {
            if (currentBitmapFile != null)
            {
                Bitmap bitmap = currentBitmapFile;

                // start time
                Stopwatch stopwatch = Stopwatch.StartNew();

                // convert to binary
                string binaryString = BmpToBinaryString(bitmap);
                Console.WriteLine(binaryString);

                // convert to ascii
                string asciiString = BinaryToAscii(binaryString);
                Console.WriteLine(asciiString);


                // implement algorithm
                if (isBM)
                {
                    // Implement BM Algorithm here

                }
                else
                {
                    // Implement KMP Algorithm here

                }

                // stop time
                stopwatch.Stop();


                // RESULT SECTION

                // count time execution
                long executionTimeMs = stopwatch.ElapsedMilliseconds;
                string executionTime = executionTimeMs.ToString();
                TextBoxRuntime.Text = $"Runtime : {executionTime}ms";

                // count percentage
                string percentage = "97";
                TextBoxSimilarityResult.Text = $"Similarity : {percentage}%";

                // take data from database
                RetrieveData("Jn Smth"); // example

                // take image from database
                RetrieveImage("Jane Smith"); // example
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
            string query = "SELECT berkas_citra " +
                "FROM sidik_jari " +
                $"WHERE nama = '{name}' " +
                "LIMIT 1;";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string relativePath = "../../../../" + reader["berkas_citra"].ToString();
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
                    else
                    {
                        MessageBox.Show("No data found in the database.");
                    }
                }
            }
        }

        private string BmpToBinaryString(Bitmap bmp)
        {
            Bitmap grayscaleBmp = new Bitmap(bmp.Width, bmp.Height);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    System.Drawing.Color pixelColor = bmp.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.299 + pixelColor.G * 0.587 + pixelColor.B * 0.114);
                    grayscaleBmp.SetPixel(x, y, System.Drawing.Color.FromArgb(grayValue, grayValue, grayValue));
                }
            }

            int threshold = 128;
            StringBuilder binaryString = new StringBuilder();

            for (int y = 0; y < grayscaleBmp.Height; y++)
            {
                for (int x = 0; x < grayscaleBmp.Width; x++)
                {
                    System.Drawing.Color grayPixelColor = grayscaleBmp.GetPixel(x, y);
                    int grayValue = grayPixelColor.R;
                    binaryString.Append(grayValue < threshold ? '1' : '0');
                }
                binaryString.Append('\n');
            }

            return binaryString.ToString();
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
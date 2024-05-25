using System;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

/* Custom styling:
 * https://www.youtube.com/watch?v=u8SL5g9QGcI&list=PLwG-AtjFaHdMQtyReCzPdEe6fZ57TqJUs&index=2
 */

namespace src
{
    public partial class Form1 : Form
    {
        private SQLiteConnection connection;
        public Form1()
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
            string sqlFilePath = "../../tubes3_stima24.sql";
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void customButton1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap Files|*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBoxUploadedImage.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        private void RetrieveData(string name)
        {
            // query here
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

                        // Assign retrieved data to your variables
                        resultLabel.Text = $"HASIL \n" +
                            $"Nama : {nameResult}\n" +
                            $"Tempat Lahir : {tempatLahirResult}\n" +
                            $"Tanggal Lahir : {tanggalLahirResult}\n" +
                            $"Jenis Kelamin : {jenisKelaminResult}\n" +
                            $"Golongan Darah : {golonganDarahResult}\n" +
                            $"Alamat : {alamatResult}\n" +
                            $"Agama : {agamaResult}\n" +
                            $"Status Perkawinan : {statusPerkawinanResult}\n" +
                            $"Pekerjaan : {pekerjaanResult}\n" +
                            $"Kewarganegaraan : {kewarganegaraanResult}";

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
                        string resultImagePath = reader["berkas_citra"].ToString();

                        if (System.IO.File.Exists(resultImagePath))
                        {
                            pictureBox2.Image = new Bitmap(resultImagePath);
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

        private void customButton2_Click(object sender, EventArgs e)
        {
            // serach button
            if (pictureBoxUploadedImage.Image != null)
            {
                Bitmap bitmap = new Bitmap(pictureBoxUploadedImage.Image);

                // start time
                Stopwatch stopwatch = Stopwatch.StartNew();

                string binaryString = BmpToBinaryString(bitmap);
                Console.WriteLine(binaryString);
                string asciiString = BinaryToAscii(binaryString);
                Console.WriteLine(asciiString);

                stopwatch.Stop();

                // take data from database
                RetrieveData("Jn Smth");

                long executionTimeMs = stopwatch.ElapsedMilliseconds;

                string executionTime = executionTimeMs.ToString();
                string percentage = "97";

                timeExecutionLabel.Text = $"Waktu pencarian : {executionTime}ms";
                label2.Text = $"Persentase Kecocokan : {percentage}%";


                // take image
                RetrieveImage("Jane Smith");
            }
            else
            {
                MessageBox.Show("Please upload an image first.");
            }
        }

        private string BmpToBinaryString(Bitmap bmp)
        {
            Bitmap grayscaleBmp = new Bitmap(bmp.Width, bmp.Height);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color pixelColor = bmp.GetPixel(x, y);
                    int grayValue = (int)(pixelColor.R * 0.299 + pixelColor.G * 0.587 + pixelColor.B * 0.114);
                    grayscaleBmp.SetPixel(x, y, Color.FromArgb(grayValue, grayValue, grayValue));
                }
            }

            int threshold = 128;
            StringBuilder binaryString = new StringBuilder();

            for (int y = 0; y < grayscaleBmp.Height; y++)
            {
                for (int x = 0; x < grayscaleBmp.Width; x++)
                {
                    Color grayPixelColor = grayscaleBmp.GetPixel(x, y);
                    int grayValue = grayPixelColor.R;
                    binaryString.Append(grayValue < threshold ? '1' : '0');
                }
                binaryString.Append('\n');
            }

            return binaryString.ToString();
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

        private void label1_Click_1(object sender, EventArgs e)
        {
            // title
        }

        private void pictureBoxUploadedImage_Click(object sender, EventArgs e)
        {
            // uploaded image
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // image result
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            // result description
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void result_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void toggleButton2_CheckedChanged(object sender, EventArgs e)
        {
            // toggle button
            if(toggleButton2.Checked)
            {
                resultLabel.Text = "Ini BM";
            }
            else
            {
                resultLabel.Text = "Ini KMP";
            }
        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click_2(object sender, EventArgs e)
        {

        }

        private void resultLabel_Click(object sender, EventArgs e)
        {

        }
    }
}

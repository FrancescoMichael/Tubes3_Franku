using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// using MySql.Data.MySqlClient;

/* Custom styling:
 * https://www.youtube.com/watch?v=u8SL5g9QGcI&list=PLwG-AtjFaHdMQtyReCzPdEe6fZ57TqJUs&index=2
 */

namespace src
{
    public partial class Form1 : Form
    {
        // private MySqlConnection connection;
        public Form1()
        {
            InitializeComponent();
            //InitializeDatabaseConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            // upload button
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap Files|*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBoxUploadedImage.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            // serach button
            if (pictureBoxUploadedImage.Image != null)
            {
                Bitmap bitmap = new Bitmap(pictureBoxUploadedImage.Image);
                string binaryString = BmpToBinaryString(bitmap);
                string asciiString = BinaryToAscii(binaryString);
                Console.WriteLine(asciiString);
                resultLabel.Text = "Hasilnya adalah : \nHalo\nHehe\nHihi";
                // ConnectionDatabase(asciiString);

                resultLabel.Font = new Font("Arial", 12, FontStyle.Bold);

                // Optionally, adjust the size to fit the content
                resultLabel.AutoSize = true;
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
            }

            return binaryString.ToString();
        }

        private string BinaryToAscii(string binaryString)
        {
            StringBuilder asciiString = new StringBuilder();

            for (int i = 0; i < binaryString.Length; i += 8)
            {
                if (i + 8 <= binaryString.Length)
                {
                    string byteString = binaryString.Substring(i, 8);
                    int decimalValue = Convert.ToInt32(byteString, 2);
                    asciiString.Append((char)decimalValue);
                }
            }

            return asciiString.ToString();
        }

        /* private void InitializeDatabaseConnection()
        {
            string connectionString = "server=your_server_address;" +
                "user=your_username;" +
                "database=your_database;" +
                "port=3307;" +
                "password=your_password";
            connection = new MySqlConnection(connectionString);
        }

        private void ConnectionDatabase(String asciiString)
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO sidik_jari (berkas_citra, nama) VALUES (@berkas_citra, @nama)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@berka_citra", asciiString);
                cmd.Parameters.AddWithValue("@nama", "TestNama");
                cmd.ExecuteNonQuery();
                MessageBox.Show("SUCCESS");
            }  
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message);
            } 
            finally
            {
                connection.Close();
            }
        } */

        private void label1_Click_1(object sender, EventArgs e)
        {
            // title
        }

        private void toggleButton1_CheckedChanged(object sender, EventArgs e)
        {
            // toggle button
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
    }
}

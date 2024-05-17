using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace src
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // upload button
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBoxUploadedImage.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBoxUploadedImage.Image != null)
            {
                Bitmap bitmap = new Bitmap(pictureBoxUploadedImage.Image);
                string binaryString = BmpToBinaryString(bitmap);
                string asciiString = BinaryToAscii(binaryString);
                Console.WriteLine(asciiString);
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


        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

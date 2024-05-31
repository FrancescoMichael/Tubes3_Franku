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


namespace FrankuGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SQLiteConnection connection;
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
            MessageBox.Show(Directory.GetCurrentDirectory());
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSelectImageHandle(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap Files|*.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bit = new BitmapImage(new Uri(openFileDialog.FileName));
                ImageContainerInput.Source = bit;
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
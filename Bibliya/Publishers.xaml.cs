using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;

namespace Bibliya
{
    /// <summary>
    /// Interaction logic for Publishers.xaml
    /// </summary>
    public partial class Publishers : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        public Publishers()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Open(new Books());
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Open(new Tickets());
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

        }
        private void Open(Window window)
        {
            window.Show();
            this.Hide();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            Open(new Authors());
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DataPublisher dPublisher = new DataPublisher("Добавить");
            dPublisher.DataChanged += Reload_Click;
            // Подписка на событие
            dPublisher.ShowDialog();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(IdTextBox.Text))
            {
                DataPublisher dPublisher = new DataPublisher("Изменить", IdTextBox.Text);
                dPublisher.DataChanged += Reload_Click;
                // Подписка на событие
                dPublisher.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите ID издательства для изменения.", "Предупреждение");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(con);

                sqlCon.Open();
                string deleteQuery = "DELETE FROM publisherABSU WHERE id = '" + IdTextBox.Text + "'";

                SqlCommand sqlCommand = new SqlCommand(deleteQuery, sqlCon);
                sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Вы удалили запись с id " + IdTextBox.Text);
                IdTextBox.Text = string.Empty;
                Reload_Click(sender, e);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); IdTextBox.Text = string.Empty; }
        }

        private void Reload_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    string query = "SELECT * FROM publisherABSU";

                    sqlCon.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                    DataTable publishersData = new DataTable();
                    adapter.Fill(publishersData);

                    publishersGrid.ItemsSource = publishersData.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Reload_Click(sender, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Open(new BookNreader());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Open(new BookAcceptance());
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Open(new MainWindow());
        }
    }
}

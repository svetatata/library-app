using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bibliya
{
    /// <summary>
    /// Логика взаимодействия для Authors.xaml
    /// </summary>
    public partial class Authors : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        public Authors()
        {
            InitializeComponent();
            
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(con);
            sqlCon.Open();
            string query = "INSERT INTO authorABSU (full_name) VALUES ('"+ FullnameBox.Text+ "');";
            SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
            sqlCommand.ExecuteNonQuery();
            Reload_Click(sender, e);
            MessageBox.Show("Автор " + FullnameBox.Text + "добавлен успешно");
            FullnameBox.Text = string.Empty;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Reload_Click(sender, e);
        }
        private void Open(Window window)
        {
            window.Show();
            this.Close();
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Open(new Books());
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Open(new Tickets());
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Open(new Publishers());
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(con);
            sqlCon.Open();
            string query = "UPDATE authorABSU SET full_name = '" + FullnameBox.Text + "' WHERE id = '" +IdTextBox.Text+"';";
            SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
            sqlCommand.ExecuteNonQuery();
            Reload_Click(sender, e);
            IdTextBox.Text = string.Empty;
            FullnameBox.Text = string.Empty;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query2 = "DELETE FROM [author&bookABSU] WHERE author ='" + IdTextBox.Text +"'";
                string query = "DELETE FROM authorABSU WHERE id='" + IdTextBox.Text + "'";
                SqlConnection sqlCon = new SqlConnection(con);
                SqlCommand sqlCommand2 = new SqlCommand(query2, sqlCon);
                SqlCommand sqlCommand = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                sqlCommand2.ExecuteNonQuery();
                sqlCommand.ExecuteNonQuery();

                MessageBox.Show("Вы удалили автора с id " + IdTextBox.Text);
                IdTextBox.Text = string.Empty;
                Reload_Click(sender, e);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); IdTextBox.Text = string.Empty; }
        }

        private void AuthorBooks_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(IdTextBox.Text))
            {
                BooksByAuthor bba = new BooksByAuthor();
                bba.authorID = IdTextBox.Text;
                bba.ShowDialog();
            }
            else
            {
                MessageBox.Show("Введите id автора для просмотра его книг");
            }
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    // Объединённый запрос для таблицы authorABSU и представления author_book_count_view
                    string query = @"
                SELECT 
                    a.id, 
                    a.full_name,
                    ISNULL(v.book_count, 0) AS BookCount
                FROM 
                    authorABSU a
                LEFT JOIN 
                    author_book_count_view v ON a.id = v.author_id";

                    sqlCon.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                    DataTable authorData = new DataTable();
                    adapter.Fill(authorData);

                    authorGrid.ItemsSource = authorData.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Open(new BookNreader());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Open(new BookAcceptance());
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            Open(new MainWindow());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

namespace Bibliya
{
    /// <summary>
    /// Логика взаимодействия для BooksByAuthor.xaml
    /// </summary>
    public partial class BooksByAuthor : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        public string authorID { get; set; }
        public BooksByAuthor()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Books books = new Books();
            books.Show();
            this.Close();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(bookId.Text))
            {
                try
                {
                    SqlConnection sqlCon = new SqlConnection(con);
                    string query = "INSERT INTO [author&bookABSU] (author, book) VALUES ('" + authorID + "', '" + bookId.Text + "');";
                    sqlCon.Open();
                    SqlCommand sqlCom = new SqlCommand(query, sqlCon);
                    sqlCom.ExecuteNonQuery();

                    string queryMessage = "SELECT title FROM bookABSU WHERE ISBN ='" +bookId.Text+"';";
                    SqlCommand sqlCom2 = new SqlCommand(queryMessage, sqlCon);
                    SqlDataReader reader = sqlCom2.ExecuteReader();

                    if (reader.Read()) // Читаем первую строку результата
                    {
                        string bookTitle = reader["title"].ToString();
                        MessageBox.Show("Авторство над книгой с названием " + bookTitle + " добавлено успешно");
                        bookId.Text = string.Empty;
                        Window_Loaded(sender, e);
                    }
                }
                    
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Введите ISBN");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection sqlCon = new SqlConnection(con);
                sqlCon.Open();
                string query = "SELECT * FROM [author&bookABSU] WHERE author = '" + authorID +"'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                DataTable ab = new DataTable();
                adapter.Fill(ab);

                bookByAuthorGrid.ItemsSource = ab.DefaultView;
            }   
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}

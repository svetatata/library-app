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
    /// Логика взаимодействия для BookNreader.xaml
    /// </summary>
    public partial class BookNreader : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        public BookNreader()
        {
            InitializeComponent();
            LoadReaders();
            LoadBookCopies();
        }

        private void LoadReaders()
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();
                string query = "SELECT id, CONCAT(lastname, ' ', firstname, ' ', patronymic) AS fullname FROM reader_ticketABSU";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                ReaderComboBox.ItemsSource = dt.DefaultView;
                ReaderComboBox.DisplayMemberPath = "fullname";
                ReaderComboBox.SelectedValuePath = "id";
            }
        }
        private void LoadBookCopies()
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();
                string query = "SELECT id, book_id FROM book_copyABSU WHERE status = 1"; // Только доступные экземпляры
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                BookCopyComboBox.ItemsSource = dt.DefaultView;
                BookCopyComboBox.DisplayMemberPath = "book_id";
                BookCopyComboBox.SelectedValuePath = "id";
            }
        }
        private void ReaderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void IssueBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReaderComboBox.SelectedValue == null || BookCopyComboBox.SelectedValue == null || !DatePickerIssueDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            string readerId = ReaderComboBox.SelectedValue.ToString();
            string copyId = BookCopyComboBox.SelectedValue.ToString();
            string issuanceDate = DatePickerIssueDate.SelectedDate.Value.ToString("yyyy-MM-dd");

            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();

                // Вставляем запись о выдаче книги
                string insertQuery = $"INSERT INTO book_issuanceABSU (reader_ticker, copy, date_issuance) VALUES ('{readerId}', '{copyId}', '{issuanceDate}')";
                SqlCommand insertCmd = new SqlCommand(insertQuery, sqlCon);
                insertCmd.ExecuteNonQuery();

                // Обновляем статус копии на "на руках"
                string updateQuery = $"UPDATE book_copyABSU SET status = 2 WHERE id = {copyId}";
                SqlCommand updateCmd = new SqlCommand(updateQuery, sqlCon);
                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Книга успешно выдана.");

                AdminBoard adminBoard = new AdminBoard();
                adminBoard.Show();
                this.Close();
            }
        }

        private void BookCopyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdminBoard adminBoard = new AdminBoard();
            adminBoard.Show();
            this.Close();
        }
    }
}

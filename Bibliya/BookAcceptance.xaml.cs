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
    /// Логика взаимодействия для BookAcceptance.xaml
    /// </summary>
    public partial class BookAcceptance : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        public BookAcceptance()
        {
            InitializeComponent();
            LoadReaders();
            LoadBookCopies();
        }
        // Загружаем читателей, у которых книга на руках (статус = 2 или 3)
        private void LoadReaders()
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();
                string query = "SELECT DISTINCT r.id, CONCAT(r.lastname, ' ', r.firstname, ' ', r.patronymic) AS fullname " +
                               "FROM reader_ticketABSU r " +
                               "JOIN book_issuanceABSU b ON r.id = b.reader_ticker " +
                               "JOIN book_copyABSU bc ON b.copy = bc.id "+
                               "WHERE bc.status IN (2, 3)"; // Статус книги 2 или 3 (на руках)

                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                //MessageBox.Show("Количество читателей с книгой на руках: " + dt.Rows.Count.ToString());
                ReaderComboBox.ItemsSource = dt.DefaultView;
                ReaderComboBox.DisplayMemberPath = "fullname";
                ReaderComboBox.SelectedValuePath = "id";

                ReaderComboBox.UpdateLayout();
            }
        }

        // Загружаем книги, которые на руках (статус книги = 2 или 3)
        private void LoadBookCopies()
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();
                string query = "SELECT bc.id, bc.book_id " +
                               "FROM book_copyABSU bc " +
                               "JOIN book_issuanceABSU bi ON bc.id = bi.copy " +
                               "WHERE bc.status IN (2, 3)"; // Книги на руках (статус 2 или 3)

                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                //MessageBox.Show("Количество книг на руках: " + dt.Rows.Count.ToString());
                BookCopyComboBox.ItemsSource = dt.DefaultView;
                BookCopyComboBox.DisplayMemberPath = "book_id";
                BookCopyComboBox.SelectedValuePath = "id";

                BookCopyComboBox.UpdateLayout();
            }
        }
        private void IssueBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReaderComboBox.SelectedValue == null || BookCopyComboBox.SelectedValue == null || !DatePickerAcceptDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            string readerId = ReaderComboBox.SelectedValue.ToString();
            string copyId = BookCopyComboBox.SelectedValue.ToString();
            string acceptDate = DatePickerAcceptDate.SelectedDate.Value.ToString("yyyy-MM-dd");

            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();

                // Вставляем запись о принятии книги
                string insertQuery = $"UPDATE book_issuanceABSU SET date_return = '{acceptDate}' WHERE copy = '{copyId}' AND reader_ticker = '{readerId}'";
                SqlCommand insertCmd = new SqlCommand(insertQuery, sqlCon);
                insertCmd.ExecuteNonQuery();

                // Обновляем статус книги на доступный (например, 1 - доступен)
                string updateQuery = $"UPDATE book_copyABSU SET status = 1 WHERE id = {copyId}";
                SqlCommand updateCmd = new SqlCommand(updateQuery, sqlCon);
                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Книга успешно принята.");
                AdminBoard adminBoard = new AdminBoard();
                adminBoard.Show();
                this.Close();
            }
        }

        private void ReaderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReaderComboBox.SelectedValue != null)
            {
                int readerId = Convert.ToInt32(ReaderComboBox.SelectedValue);
                LoadBookCopiesForReader(readerId); // Загружаем книги для выбранного читателя
            }
            else
            {
                LoadBookCopies(); // Если читатель не выбран, загружаем все книги
            }
        }

        // Загружаем книги только для выбранного читателя
        private void LoadBookCopiesForReader(int readerId)
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();
                string query = "SELECT bc.id, bc.book_id " +
                               "FROM book_copyABSU bc " +
                               "JOIN book_issuanceABSU bi ON bc.id = bi.copy " +
                               "WHERE bi.reader_ticker = " + readerId + " AND bc.status IN (2, 3)"; // Книги на руках у выбранного читателя

                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                BookCopyComboBox.ItemsSource = dt.DefaultView;
                BookCopyComboBox.DisplayMemberPath = "book_id";
                BookCopyComboBox.SelectedValuePath = "id";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdminBoard adminBoard = new AdminBoard();
            adminBoard.Show();
            this.Close();
        }
    }
}

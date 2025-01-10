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
    /// Логика взаимодействия для RenewBook.xaml
    /// </summary>
    public partial class RenewBook : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        public RenewBook()
        {
            InitializeComponent();
            LoadReaders();

        }
        private void LoadReaders()
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();
                string query = @"
                    SELECT r.id, CONCAT(r.lastname, ' ', r.firstname, ' ', r.patronymic) AS fullname
                    FROM reader_ticketABSU r
                    JOIN book_issuanceABSU b ON r.id = b.reader_ticker
                    JOIN book_copyABSU bc ON b.copy = bc.id
                    WHERE bc.status IN (2, 3)"; // Статус книги 2 или 3 (на руках)

                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                readerBox.ItemsSource = dt.DefaultView;
                readerBox.DisplayMemberPath = "fullname";
                readerBox.SelectedValuePath = "id";

                readerBox.UpdateLayout();
            }
        }
        private void LoadBooksForReader(int readerId)
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();
                string query = @"
                    SELECT bc.id, bc.book_id, b.title
                    FROM book_copyABSU bc
                    JOIN book_issuanceABSU bi ON bc.id = bi.copy
                    JOIN bookABSU b ON bc.book_id = b.ISBN
                    WHERE bi.reader_ticker = " + readerId + " AND bc.status IN (2, 3)"; // Книги на руках у выбранного читателя

                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                ISBNBox.ItemsSource = dt.DefaultView;
                ISBNBox.DisplayMemberPath = "title";  // Отображаем название книги
                ISBNBox.SelectedValuePath = "book_id";  // В качестве значения используем book_id

                ISBNBox.UpdateLayout();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ClientBoard clientBoard = new ClientBoard();
            clientBoard.Show();
            this.Close();
        }

        private void RenewButton_Click(object sender, RoutedEventArgs e)
        {
            if (readerBox.SelectedValue == null || ISBNBox.SelectedValue == null || !ReturnDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            string readerId = readerBox.SelectedValue.ToString();
            string bookId = ISBNBox.SelectedValue.ToString();
            string returnDate = ReturnDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");

            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();

                string updateQuery = $"UPDATE book_issuanceABSU SET date_return = '{returnDate}' WHERE copy = '{bookId}' AND reader_ticker = '{readerId}'";
                SqlCommand updateCmd = new SqlCommand(updateQuery, sqlCon);
                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Книга успешно продлена.");
                ClientBoard clientBoard = new ClientBoard();
                clientBoard.Show();
                this.Close();
            }

        }

        private void readerBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (readerBox.SelectedValue != null)
            {
                int readerId = Convert.ToInt32(readerBox.SelectedValue);
                LoadBooksForReader(readerId); // Загружаем книги для выбранного читателя
            }
            else
            {
                ISBNBox.ItemsSource = null; // Если читатель не выбран, очищаем список книг
            }
        }
    }
}
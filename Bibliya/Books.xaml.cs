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
    /// Логика взаимодействия для Books.xaml
    /// </summary>
    public partial class Books : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        public Books()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Open(new Tickets());
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Open(new Publishers());
        }
        private void Open(Window window)
        {
            window.Show();
            this.Close();
        }

        private void board_Click(object sender, RoutedEventArgs e)
        {
            Open(new AdminBoard());
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(id.Text))
            {
                string bookIsbn = id.Text;
                SqlConnection sqlCon = new SqlConnection(con);
                try
                {
                    // 1. Подсчитываем количество записей, которые будут удалены
                    string countBookIssuanceQuery = "SELECT COUNT(*) FROM book_issuanceABSU WHERE copy IN (SELECT id FROM book_copyABSU WHERE book_id = '" + bookIsbn + "')";
                    SqlCommand countBookIssuanceCmd = new SqlCommand(countBookIssuanceQuery, sqlCon);

                    string countBookCopyQuery = "SELECT COUNT(*) FROM book_copyABSU WHERE book_id = '" + bookIsbn + "'";
                    SqlCommand countBookCopyCmd = new SqlCommand(countBookCopyQuery, sqlCon);

                    string countAuthorBookQuery = "SELECT COUNT(*) FROM [author&bookABSU] WHERE book = '" + bookIsbn + "'";
                    SqlCommand countAuthorBookCmd = new SqlCommand(countAuthorBookQuery, sqlCon);

                    string countBookQuery = "SELECT COUNT(*) FROM bookABSU WHERE ISBN = '" + bookIsbn + "'";
                    SqlCommand countBookCmd = new SqlCommand(countBookQuery, sqlCon);

                    // Открываем соединение для подсчета
                    sqlCon.Open();

                    int bookIssuanceCount = (int)countBookIssuanceCmd.ExecuteScalar();
                    int bookCopyCount = (int)countBookCopyCmd.ExecuteScalar();
                    int authorBookCount = (int)countAuthorBookCmd.ExecuteScalar();
                    int bookCount = (int)countBookCmd.ExecuteScalar();

                    // Закрываем соединение после подсчета
                    sqlCon.Close();

                    // Формируем сообщение с предупреждением
                    string warningMessage = "Внимание! Будет удалено:\n";
                    warningMessage += $"{bookIssuanceCount} записи о выдаче книги\n";
                    warningMessage += $"{bookCopyCount} копий книги\n";
                    warningMessage += $"{authorBookCount} связи с авторами\n";
                    warningMessage += $"1 книга (ISBN: {bookIsbn})\n";

                    // Запрашиваем подтверждение у пользователя
                    MessageBoxResult result = MessageBox.Show(warningMessage, "Подтверждение удаления", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Перезапускаем соединение для удаления
                        sqlCon.Open();

                        // 2. Удаляем все записи из book_issuanceABSU, связанные с книгой
                        string deleteBookIssuanceQuery = "DELETE FROM book_issuanceABSU WHERE copy IN (SELECT id FROM book_copyABSU WHERE book_id = '" + bookIsbn + "')";
                        SqlCommand sqlCommand1 = new SqlCommand(deleteBookIssuanceQuery, sqlCon);

                        // 3. Удаляем все копии книги из book_copyABSU
                        string deleteBookCopyQuery = "DELETE FROM book_copyABSU WHERE book_id = '" + bookIsbn + "'";
                        SqlCommand sqlCommand2 = new SqlCommand(deleteBookCopyQuery, sqlCon);

                        // 4. Удаляем связи книги с авторами из author&bookABSU
                        string deleteAuthorBookQuery = "DELETE FROM [author&bookABSU] WHERE book = '" + bookIsbn + "'";
                        SqlCommand sqlCommand3 = new SqlCommand(deleteAuthorBookQuery, sqlCon);

                        // 5. Удаляем саму книгу из bookABSU
                        string deleteBookQuery = "DELETE FROM bookABSU WHERE ISBN = '" + bookIsbn + "'";
                        SqlCommand sqlCommand4 = new SqlCommand(deleteBookQuery, sqlCon);

                        // Выполняем удаление записей
                        sqlCommand1.ExecuteNonQuery();  // Удаляем записи из book_issuanceABSU
                        sqlCommand2.ExecuteNonQuery();  // Удаляем копии из book_copyABSU
                        sqlCommand3.ExecuteNonQuery();  // Удаляем связи из author&bookABSU
                        sqlCommand4.ExecuteNonQuery();  // Удаляем книгу из bookABSU

                        MessageBox.Show("Вы удалили книгу с ISBN " + bookIsbn);
                        id.Text = string.Empty;
                        reload_Click(sender, e);  // Перезагружаем данные
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); id.Text = string.Empty; }

            }
            else
            {
                MessageBox.Show("Введите id книги для удаления");
            }
            
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(id.Text))
            {
                DataBook dbook = new DataBook("Изменить", id.Text);
                dbook.DataChanged += reload_Click;
                // Подписка на событие
                dbook.ShowDialog();
                id.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите ISBN книги для изменения.", "Предупреждение");
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            DataBook dbook = new DataBook("Добавить");
            dbook.DataChanged += reload_Click;
            // Подписка на событие
            dbook.ShowDialog();
        }

        private void Chan_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Когда текст в поисковом поле меняется, обновляем отображение
            string searchTerm = Search.Text.Trim();
            ReloadBooks(searchTerm);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            reload_Click(sender, e);

        }

        
        private void ReloadBooks(string searchTerm)
        {
            try
            {
                // Закрываем соединение и очищаем текущий источник данных
                bookGrid.ItemsSource = null;

                string query = "SELECT * FROM bookABSU WHERE title LIKE '%" + searchTerm + "%' OR ISBN LIKE '%" + searchTerm + "%'OR authors LIKE '%" + searchTerm + "%'";

                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    sqlCon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                    DataTable bookTable = new DataTable();
                    adapter.Fill(bookTable);

                    // Присваиваем результат в DataGrid
                    bookGrid.ItemsSource = bookTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Search_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void reload_Click(object sender, EventArgs e)
        {

            try
            {
                bookGrid.ItemsSource = null;
                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    string query = "SELECT * FROM bookABSU";
                    sqlCon.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                    DataTable bookTable = new DataTable();
                    adapter.Fill(bookTable);

                    bookGrid.ItemsSource = bookTable.DefaultView;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void copies_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(id.Text))
            {
                string isbn = id.Text;
                DataCopy dataCopy = new DataCopy();
                dataCopy.BookISBN = isbn;
                dataCopy.ShowDialog();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите ISBN книги для просмотра копий.", "Предупреждение");
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            Open(new Authors());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Open(new BookNreader());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Open(new BookAcceptance());
        }

        public void ApplyFilters(List<int> selectedAuthors, int yearFrom, int yearTo, int selectedCategoryId, bool onlyAvailable)
        {
            try
            {
                bookGrid.ItemsSource = null;

                // Строим базовый запрос
                var query = new StringBuilder("SELECT * FROM bookABSU WHERE 1=1");

                // Авторы
                if (selectedAuthors.Any())
                {
                    query.Append($" AND ISBN IN (SELECT book FROM [author&bookABSU] WHERE author IN ({string.Join(",", selectedAuthors)}))");
                }

                // Годы
                if (yearFrom > 0 && yearTo > 0)
                {
                    query.Append(" AND publishing_year BETWEEN @YearFrom AND @YearTo");
                }

                // Категория
                if (selectedCategoryId > 0)
                {
                    query.Append(" AND category_id = @CategoryId");
                }

                // Только доступные
                if (onlyAvailable)
                {
                    query.Append(" AND ISBN IN (SELECT book_id FROM book_copyABSU WHERE status = 1)");
                }

                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    sqlCon.Open();
                    var cmd = new SqlCommand(query.ToString(), sqlCon);

                    // Параметры
                    if (yearFrom > 0 && yearTo > 0)
                    {
                        cmd.Parameters.AddWithValue("@YearFrom", yearFrom);
                        cmd.Parameters.AddWithValue("@YearTo", yearTo);
                    }

                    if (selectedCategoryId > 0)
                    {
                        cmd.Parameters.AddWithValue("@CategoryId", selectedCategoryId);
                    }

                    var adapter = new SqlDataAdapter(cmd);
                    var bookTable = new DataTable();
                    adapter.Fill(bookTable);

                    bookGrid.ItemsSource = bookTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Filters filterWindow = new Filters(this);
            filterWindow.ShowDialog();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Open(new MainWindow());
        }
    }
}

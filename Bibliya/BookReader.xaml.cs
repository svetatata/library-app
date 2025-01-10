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
    /// Логика взаимодействия для BookReader.xaml
    /// </summary>
    public partial class BookReader : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        public BookReader()
        {
            InitializeComponent();
        }
        private void Open(Window window)
        {
            window.Show();
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Open (new ClientBoard());
        }

        private void Chan_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = Search.Text.Trim();
            ReloadBooks(searchTerm);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Filters filterWindow = new Filters(this);
            filterWindow.ShowDialog();
        }

        private void bookGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
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
        private void ReloadBooks(string searchTerm)
        {
            try
            {
                // Закрываем соединение и очищаем текущий источник данных
                bookGrid.ItemsSource = null;

                string query = "SELECT * FROM bookABSU WHERE title LIKE '%" + searchTerm + "%' OR ISBN LIKE '%" + searchTerm + "%'";

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
        public void ApplyFilters(List<int> selectedAuthors, int yearFrom, int yearTo, int selectedCategoryId, bool onlyAvailable)
        {
            try
            {
                // Очищаем текущие данные в DataGrid
                bookGrid.ItemsSource = null;

                // Строим базовый запрос
                StringBuilder query = new StringBuilder("SELECT * FROM bookABSU WHERE 1=1");

                // Если выбраны авторы, добавляем условие для авторов
                if (selectedAuthors.Any())
                {
                    query.Append(" AND ISBN IN (SELECT book FROM [author&bookABSU] WHERE author IN (" + string.Join(",", selectedAuthors) + "))");
                }

                // Если указан диапазон лет, добавляем условие для года
                if (yearFrom > 0 && yearTo > 0)
                {
                    query.Append(" AND publishing_year BETWEEN @YearFrom AND @YearTo");
                }

                // Если указана категория, добавляем условие для категории
                if (selectedCategoryId > 0)
                {
                    query.Append(" AND category_id = @CategoryId");
                }

                // Если фильтр по доступности, добавляем условие для доступных книг
                if (onlyAvailable)
                {
                    query.Append(" AND ISBN IN (SELECT DISTINCT book_id FROM book_copyABSU WHERE status = 1)");
                }

                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    sqlCon.Open();

                    SqlCommand cmd = new SqlCommand(query.ToString(), sqlCon);

                    // Добавляем параметры для запроса, если это необходимо
                    if (yearFrom > 0 && yearTo > 0)
                    {
                        cmd.Parameters.AddWithValue("@YearFrom", yearFrom);
                        cmd.Parameters.AddWithValue("@YearTo", yearTo);
                    }
                    if (selectedCategoryId > 0)
                    {
                        cmd.Parameters.AddWithValue("@CategoryId", selectedCategoryId);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
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

    }
}

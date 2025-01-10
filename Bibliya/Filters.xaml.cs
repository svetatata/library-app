using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Логика взаимодействия для Filters.xaml
    /// </summary>
    public partial class Filters : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        private Window _parentWindow;
        public Filters(Window parentWindow)
        {
            InitializeComponent();
            LoadAuthors();
            LoadCategories();
            _parentWindow = parentWindow;
        }

        private void LoadAuthors()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT id, full_name FROM authorABSU", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        AuthorsListBox.Items.Add(new CheckBox
                        {
                            Content = reader["full_name"].ToString(),
                            Tag = Convert.ToInt32(reader["id"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT id, name FROM categoryABSU", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        CategoryComboBox.Items.Add(new ComboBoxItem
                        {
                            Content = reader["name"].ToString(),
                            Tag = Convert.ToInt32(reader["id"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ApplyFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получение авторов
                var selectedAuthors = AuthorsListBox.Items.OfType<CheckBox>()
                    .Where(c => c.IsChecked == true)
                    .Select(c => (int)c.Tag).ToList();

                // Проверка на корректность ввода годов
                if (!int.TryParse(YearFromTextBox.Text, out int yearFrom)) yearFrom = 0;
                if (!int.TryParse(YearToTextBox.Text, out int yearTo)) yearTo = 0;

                // Категория
                int selectedCategoryId = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Tag as int? ?? -1;

                // Только в наличии
                bool onlyAvailable = OnlyAvailableCheckBox.IsChecked == true;

                // Передача в главное окно
                if (_parentWindow is Books mainWindow)
                {
                    mainWindow.ApplyFilters(selectedAuthors, yearFrom, yearTo, selectedCategoryId, onlyAvailable);
                }
                else if (_parentWindow is BookReader mainWindowReader) {
                    mainWindowReader.ApplyFilters(selectedAuthors, yearFrom, yearTo, selectedCategoryId, onlyAvailable);
                }
                else
                {
                    MessageBox.Show("Главное окно не найдено.");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

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
    /// Логика взаимодействия для DataCopy.xaml
    /// </summary>
    public partial class DataCopy : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        public string BookISBN { get; set; }
        public DataCopy()
        {
            InitializeComponent();
            LoadData();
            LoadStatus();
        }

        private void LoadStatus()
        {
            string statusQuery = "SELECT id, name FROM status_copyABSU"; 

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    sqlCon.Open();

                    SqlCommand statusCommand = new SqlCommand(statusQuery, sqlCon);
                    SqlDataReader statusReader = statusCommand.ExecuteReader();

                    status.Items.Clear(); // Очистим ComboBox перед добавлением новых элементов

                    while (statusReader.Read())
                    {
                        string displayText = statusReader["name"].ToString();
                        int statusID = (int)statusReader["id"];

                        // Создаем новый объект Status и добавляем в ComboBox
                        status.Items.Add(new Status { Text = displayText, Id = statusID });
                    }

                    // Настройка правильных путей для отображения
                    status.DisplayMemberPath = "Text";
                    status.SelectedValuePath = "Id";

                    statusReader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                copyGrid.ItemsSource = null;

                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    sqlCon.Open();

                    // Запрос для получения данных копий книги по ISBN
                    string query = "SELECT * FROM book_copyABSU WHERE book_id = '" + BookISBN + "'";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                    DataTable copyTable = new DataTable();
                    adapter.Fill(copyTable);

                    copyGrid.ItemsSource = copyTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string recordId = id.Text;
                string newInventoryNumber = inventory_num.Text;
                int newStatus = (int)status.SelectedValue; // Получаем выбранный статус из ComboBox

                // Проверяем, чтобы ID и Inventory не были пустыми
                if (string.IsNullOrEmpty(recordId) || string.IsNullOrEmpty(newInventoryNumber))
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }

                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    sqlCon.Open();

                    // Формируем запрос на обновление данных
                    string updateQuery = "UPDATE book_copyABSU SET inventory_number = @inventoryNumber, status = @status WHERE id = @id";

                    SqlCommand cmd = new SqlCommand(updateQuery, sqlCon);
                    cmd.Parameters.AddWithValue("@inventoryNumber", newInventoryNumber);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@id", recordId);

                    // Выполняем обновление данных
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Изменения успешно сохранены.");
                        LoadData(); // Перезагружаем данные в DataGrid
                    }
                    else
                    {
                        MessageBox.Show("Запись с таким ID не найдена.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        public class Status
        {
            public string Text { get; set; }
            public int Id { get; set; }
        }
    }
}

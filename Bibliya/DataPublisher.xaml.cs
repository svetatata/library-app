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
    /// Логика взаимодействия для DataPublisher.xaml
    /// </summary>
    public partial class DataPublisher : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string operation;
        string publisherId;
        public DataPublisher(string operation, string publisherId = null)
        {
            InitializeComponent();
            this.operation = operation;
            this.publisherId = publisherId;

            if (operation == "Изменить" && !string.IsNullOrEmpty(publisherId))
            {
                LoadPublData(publisherId);
            }
        }

        private void LoadPublData(string publisherId)
        {
            SqlConnection sqlCon = new SqlConnection(con);


            sqlCon.Open();
            string query = "SELECT id, name, town " +
                            "FROM publisherABSU WHERE id = '" + publisherId + "'";

            SqlCommand command = new SqlCommand(query, sqlCon);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                id.Text = reader["id"].ToString();
                nameBox.Text = reader["name"].ToString();
                townBox.Text = reader["town"].ToString();
            }
        }

        protected virtual void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged(this, EventArgs.Empty);
            }
        }
        public event EventHandler DataChanged;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sqlCon = new SqlConnection(con);
            
            sqlCon.Open();
            string query;

            if (operation == "Добавить")
            {
                query = "INSERT INTO publisherABSU (name, town) " +
                            "VALUES ('" + nameBox.Text + "', '" + townBox.Text + "');";
            }
            else // Изменить
            {
                query = "UPDATE publisherABSU SET " +
                        "name = '" + nameBox.Text + "', " +
                        "town = '" + townBox.Text + "' " +
                        "WHERE id = '" + id.Text + "'";
            }
            SqlCommand command = new SqlCommand(query, sqlCon);
            command.ExecuteNonQuery();

            MessageBox.Show($"{(operation == "Добавить" ? "Издатель добавлен" : "Издатель обновлен")} успешно!", "Успех");
            id.Text = string.Empty;
            OnDataChanged();

            Close();
        }
    }
}

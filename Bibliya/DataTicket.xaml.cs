using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
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
    /// Логика взаимодействия для DataTicket.xaml
    /// </summary>
    public partial class DataTicket : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string operation;
        string ticketId;
        public event EventHandler DataChanged;
        protected virtual void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged(this, EventArgs.Empty);
            }
        }
        public DataTicket(string operation, string ticketId = null)
        {
            InitializeComponent();
            this.operation = operation;
            this.ticketId = ticketId;

            if (operation == "Изменить" && !string.IsNullOrEmpty(ticketId))
            {
                LoadTicketData(ticketId);
            }
        }

        private void LoadTicketData(string ticketId)
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();
                string query = "SELECT id, lastname, firstname, patronymic, passport " +
                               "FROM reader_ticketABSU WHERE id = '" + ticketId + "'";

                SqlCommand command = new SqlCommand(query, sqlCon);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    id.Text = reader["id"].ToString();
                    lastnameBox.Text = reader["lastname"].ToString();
                    firstnameBox.Text = reader["firstname"].ToString();
                    patronymicBox.Text = reader["patronymic"].ToString();
                    passportBox.Text = reader["passport"].ToString();

                    id.IsEnabled = false;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();
                string query;

                if (operation == "Добавить")
                {
                    if (!string.IsNullOrEmpty(id.Text)) // Если ID задано явно
                    {
                        // Включаем IDENTITY_INSERT
                        string enableIdentityInsert = "SET IDENTITY_INSERT reader_ticketABSU ON;";
                        SqlCommand enableCommand = new SqlCommand(enableIdentityInsert, sqlCon);
                        enableCommand.ExecuteNonQuery();

                        // Выполняем запрос с явным ID
                        query = "INSERT INTO reader_ticketABSU (id, lastname, firstname, patronymic, passport) " +
                                "VALUES ('" + id.Text + "', '" + lastnameBox.Text + "', '" + firstnameBox.Text + "', '" + patronymicBox.Text + "', '" + passportBox.Text + "');";
                        SqlCommand command = new SqlCommand(query, sqlCon);
                        command.ExecuteNonQuery();

                        // Отключаем IDENTITY_INSERT
                        string disableIdentityInsert = "SET IDENTITY_INSERT reader_ticketABSU OFF;";
                        SqlCommand disableCommand = new SqlCommand(disableIdentityInsert, sqlCon);
                        disableCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        // Запрос без явного указания ID
                        query = "INSERT INTO reader_ticketABSU (lastname, firstname, patronymic, passport) " +
                                "VALUES ('" + lastnameBox.Text + "', '" + firstnameBox.Text + "', '" + patronymicBox.Text + "', '" + passportBox.Text + "');";
                        SqlCommand command = new SqlCommand(query, sqlCon);
                        command.ExecuteNonQuery();
                    }
                }
                else // Изменить
                {
                    query = "UPDATE reader_ticketABSU SET " +
                            "lastname = '" + lastnameBox.Text + "', " +
                            "firstname = '" + firstnameBox.Text + "', " +
                            "patronymic = '" + patronymicBox.Text + "', " +
                            "passport = '" + passportBox.Text + "' " +
                            "WHERE id = '" + id.Text + "';";
                    SqlCommand command = new SqlCommand(query, sqlCon);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show($"{(operation == "Добавить" ? "Читатель добавлен" : "Читетель обновлен")} успешно!", "Успех");
                id.Text = string.Empty;
                OnDataChanged();

                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

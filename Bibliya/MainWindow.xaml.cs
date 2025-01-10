using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bibliya
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void ClearAll()
        {
            EmailTextBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            PasswordTextBox.Text= string.Empty;
        }

        private void Open(Window window)
        {
            window.Show();
            this.Close();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string passwordText = PasswordTextBox.Text;
            if (PasswordTextBox.Visibility == Visibility.Collapsed)
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Пожалуйста, введите логин и пароль.");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passwordText))
                {
                    MessageBox.Show("Пожалуйста, введите логин и пароль.");
                    return;
                }
            }
            

            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                try
                {
                    sqlCon.Open();
                    string query;
                    if (PasswordBox.Visibility == Visibility.Collapsed)
                    {
                        query = "SELECT username, password, role FROM usersABSU WHERE username = '" + username + "' AND password = '" + passwordText + "'";
                    }
                    else
                    {
                        query = "SELECT username, password, role FROM usersABSU WHERE username = '" + username + "' AND password = '" + password + "'";
                    }
                    SqlCommand cmd = new SqlCommand(query, sqlCon);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read(); // Читаем данные

                        int role = reader.GetInt32(2);  // Получаем роль пользователя

                        if (role == 1) // 1 - администратор
                        {
                            MessageBox.Show("Вы - администратор");
                            Open(new AdminBoard());
                        }
                        else if (role == 2) // 2 - клиент
                        {
                            MessageBox.Show("Вы - клиент");
                            Open(new ClientBoard());
                        }
                        else
                        {
                            MessageBox.Show("Неизвестная роль пользователя");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при подключении к базе данных: {ex.Message}");
                }
            }

            ClearAll();

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Open(new Reg());
        }

        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Text = PasswordBox.Password;  // Копируем введённый пароль в текстовый бокс
            PasswordTextBox.Visibility = Visibility.Visible;  // Делаем видимым текстовый бокс
            PasswordBox.Visibility = Visibility.Collapsed;  // Прячем PasswordBox
        }

        private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = PasswordTextBox.Text;  // Копируем текст из текстового бокса обратно в PasswordBox
            PasswordBox.Visibility = Visibility.Visible;   // Показываем PasswordBox
            PasswordTextBox.Visibility = Visibility.Collapsed;  // Прячем текстовый бокс
        }
    }
}

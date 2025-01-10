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
    /// Логика взаимодействия для Reg.xaml
    /// </summary>
    public partial class Reg : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        private Dictionary<string, int> roles;
        public Reg()
        {
            InitializeComponent();
            roles = new Dictionary<string, int>
            {
                { "Администратор", 1 },
                { "Читатель", 2 },
            };
            RoleComboBox.Items.Clear();
            RoleComboBox.ItemsSource = roles.Keys;
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string role = RoleComboBox.SelectedItem.ToString();
            string nickname = NicknameTextBox.Text;
            string password = PasswordBox.Password;
            string repeatPassword = RepeatPasswordBox.Password;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(RepeatPasswordBox.Password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
                return;
            }

            if (password != repeatPassword)
            {
                MessageBox.Show("Пароли не совпадают. Пожалуйста, введите одинаковые пароли.");
                return;
            }
            int roleId = roles[role];

            
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    string query = "INSERT INTO usersABSU (username, password, role) VALUES ('" + nickname + "', '" + password + "', '" + roleId + "' )";
                    sqlCon.Open();
                    SqlCommand command = new SqlCommand(query, sqlCon);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Пользователь с ником " + nickname + " успешно зарегистрирован под ролью " + role);
                    if (roleId == 1) {
                        Open(new AdminBoard());
                    }
                    else
                    {
                        Open(new ClientBoard());
                    }
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void ToAuthorize_Click(object sender, RoutedEventArgs e)
        {
            Open(new MainWindow());
        }
        private void Open(Window window)
        {
            window.Show();
            this.Close();
        }
    }
}

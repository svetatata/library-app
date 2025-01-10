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
        /// Interaction logic for Tickets.xaml
        /// </summary>
        public partial class Tickets : Window
        {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        public Tickets()
            {
                InitializeComponent();
            
            }

            private void MenuItem_Click(object sender, RoutedEventArgs e)
            {

            }

            private void MenuItem_Click_1(object sender, RoutedEventArgs e)
            {
                Open(new Publishers());
            }

            private void MenuItem_Click_2(object sender, RoutedEventArgs e)
            {
                Open(new Books());
            }
            private void Open(Window window)
            {
                window.Show();
                this.Close();
            }

            private void Add_Click(object sender, RoutedEventArgs e)
            {
                DataTicket dTicket = new DataTicket("Добавить");
                dTicket.DataChanged += Reload_Click;
                // Подписка на событие
                dTicket.ShowDialog();
            }

            private void Change_Click(object sender, RoutedEventArgs e)
            {
                if (!string.IsNullOrWhiteSpace(IdTextBox.Text))
                {
                    DataTicket dTicket = new DataTicket("Изменить", IdTextBox.Text);
                    dTicket.DataChanged += Reload_Click;
                    // Подписка на событие
                    dTicket.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите ID читательского билета для изменения.", "Предупреждение");
                }
            }

            private void Delete_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    SqlConnection sqlCon = new SqlConnection(con);

                    sqlCon.Open();
                    string deleteBookIssuanceQuery = "DELETE FROM book_issuanceABSU WHERE copy IN (SELECT id FROM book_copyABSU WHERE reader_ticker = '" + IdTextBox.Text + "')";
                    string query = "DELETE FROM reader_ticketABSU WHERE id='" + IdTextBox.Text + "'";

                    SqlCommand sqlCommand2 = new SqlCommand(deleteBookIssuanceQuery, sqlCon);
                    SqlCommand sqlCommand = new SqlCommand(query, sqlCon);

                    sqlCommand2.ExecuteNonQuery();
                    sqlCommand.ExecuteNonQuery();

                    MessageBox.Show("Вы удалили запись с id " + IdTextBox.Text);
                    IdTextBox.Text = string.Empty;
                    Reload_Click(sender, e);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); IdTextBox.Text = string.Empty; }
            }

            private void Reload_Click(object sender, EventArgs e)
            {
                try
                {
                    TicketsDataGrid.ItemsSource = null;

                    using (SqlConnection sqlCon = new SqlConnection(con))
                    {
                        string query = @"
                    SELECT 
                        rt.id,
                        rt.lastname,
                        rt.firstname,
                        rt.patronymic,
                        rt.passport,
                        rtv.returned_books AS returned_books,
                        rtv.not_returned_books AS not_returned_books,
                        rtv.last_issued_isbn AS last_issued_isbn
                    FROM reader_ticketABSU rt
                    LEFT JOIN reader_ticket_view rtv ON rt.id = rtv.reader_ticket_id";

                        sqlCon.Open();

                        SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                        DataTable ticketsTable = new DataTable();
                        adapter.Fill(ticketsTable);

                        if (ticketsTable.Rows.Count > 0)
                        {
                            TicketsDataGrid.ItemsSource = ticketsTable.DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("Нет данных для отображения.");
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }

            private void Window_Loaded(object sender, RoutedEventArgs e)
            {
                Reload_Click(sender, e);
            }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            Open(new Authors());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Open(new BookNreader());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Open(new BookAcceptance());
        }

        private void Chan_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = Search.Text.Trim();
            ReloadTickets(searchTerm);
        }

        private void ReloadTickets(string searchTerm)
        {
            try
            {
                // Закрываем соединение и очищаем текущий источник данных
                TicketsDataGrid.ItemsSource = null;

                string query = "SELECT * FROM reader_ticketABSU WHERE lastname LIKE '%" + searchTerm + "%' OR firstname LIKE '%" + searchTerm + "%'OR patronymic LIKE '%" + searchTerm + "%' OR passport LIKE '%" + searchTerm + "%'";

                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    sqlCon.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, sqlCon);
                    DataTable ticketsTable = new DataTable();
                    adapter.Fill(ticketsTable);

                    // Присваиваем результат в DataGrid
                    TicketsDataGrid.ItemsSource = ticketsTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            Open(new MainWindow());
        }
    }
}

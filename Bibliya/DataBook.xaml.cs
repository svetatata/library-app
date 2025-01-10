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
    /// Interaction logic for DataBook.xaml
    /// </summary>
    public partial class DataBook : Window
    {
        private string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        string operation;
        string bookId;
        public event EventHandler DataChanged;

        protected virtual void OnDataChanged()
        {
            if (DataChanged != null)
            {
                DataChanged(this, EventArgs.Empty);
            }
        }
        public DataBook(string operation, string bookId = null)
        {
            InitializeComponent();
            LoadComboBoxes();
            this.operation = operation;
            this.bookId = bookId;

            if (operation == "Изменить" && !string.IsNullOrEmpty(bookId))
            {
                LoadBookData(bookId);
            }
        }
        private void LoadBookData(string bookId)
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();
                string query = "SELECT ISBN, title, copies, pages, publishing_year, " +
                               "(SELECT name FROM publisherABSU WHERE id = bookABSU.publisher) AS publisher, " +
                               "(SELECT name FROM categoryABSU WHERE id = bookABSU.category_id) AS category " +
                               "FROM bookABSU WHERE ISBN = '" + bookId + "'";

                SqlCommand command = new SqlCommand(query, sqlCon);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    id.Text = reader["ISBN"].ToString();
                    title.Text = reader["title"].ToString();
                    copies.Text = reader["copies"].ToString();
                    pages.Text = reader["pages"].ToString();
                    publishing_year.Text = reader["publishing_year"].ToString();
                    publisher.Text = reader["publisher"].ToString();
                    category.Text = reader["category"].ToString();

                    //id.IsEnabled = false;
                    LoadAuthorsForBook(bookId);

                    int totalCopies = GetCurrentCopiesCount(sqlCon);
                    MessageBox.Show($"Количество копий для книги '{title.Text}': {totalCopies}", "Информация");
                }
            }
        }
        private void LoadAuthorsForBook(string bookId)
        {
            string authorQuery = "SELECT a.id, a.full_name FROM authorABSU a " +
                         "INNER JOIN [author&bookABSU] ab ON a.id = ab.author " +
                         "WHERE ab.book = '" + bookId + "'"; 

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    sqlCon.Open();
                    SqlCommand authorCommand = new SqlCommand(authorQuery, sqlCon);
                    SqlDataReader authorReader = authorCommand.ExecuteReader();

                    // Добавляем авторов в динамический контейнер
                    while (authorReader.Read())
                    {
                        string authorName = authorReader["full_name"].ToString();
                        int authorId = (int)authorReader["id"];

                        // Создаем контейнер для каждого автора
                        StackPanel authorContainer = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            Margin = new Thickness(5)
                        };

                        // ComboBox для существующего автора
                        ComboBox existingAuthorComboBox = new ComboBox
                        {
                            Margin = new Thickness(5),
                            Width = 200,
                            DisplayMemberPath = "Text",
                            SelectedValuePath = "id",
                            SelectedValue = authorId
                        };
                        existingAuthorComboBox.Items.Add(new { Text = authorName, id = authorId });

                        // Кнопка для удаления автора
                        Button deleteAuthorButton = new Button
                        {
                            Content = "Удалить",
                            Width = 75,
                            Margin = new Thickness(5)
                        };
                        deleteAuthorButton.Click += (sender, e) => RemoveAuthorFromBook(authorId, authorContainer);

                        // Добавляем ComboBox и кнопку в контейнер
                        authorContainer.Children.Add(existingAuthorComboBox);
                        authorContainer.Children.Add(deleteAuthorButton);

                        // Добавляем контейнер с автором в панель
                        AuthorPanel.Children.Add(authorContainer);
                    }
                    authorReader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных авторов: " + ex.Message, "Ошибка");
            }
        }
        private void RemoveAuthorFromBook(int authorId, StackPanel authorContainer)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    sqlCon.Open();

                    // Удаление связи между автором и книгой
                    string deleteLinkQuery = "DELETE FROM [author&bookABSU] WHERE author = " + authorId + " AND book = '" + id.Text + "'";
                    SqlCommand deleteLinkCommand = new SqlCommand(deleteLinkQuery, sqlCon);
                    deleteLinkCommand.ExecuteNonQuery();

                    // Удаляем панель автора из интерфейса
                    AuthorPanel.Children.Remove(authorContainer);

                    MessageBox.Show("Автор удален успешно!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                sqlCon.Open();
                string query;

                int newCopies = int.Parse(copies.Text);
                int currentCopies = GetCurrentCopiesCount(sqlCon);
                int copiesDifference = newCopies - currentCopies;

                if (operation == "Добавить")
                {
                    if (!string.IsNullOrEmpty(id.Text))
                    {
                        query = "INSERT INTO bookABSU (ISBN, title, copies, pages, publisher, publishing_year, category_id, publishung_place, register_date) " +
                               "VALUES ('" + id.Text + "', '" + title.Text + "', '" + copies.Text + "', '" + pages.Text + "', " +
                               "(SELECT id FROM publisherABSU WHERE name = '" + publisher.Text + "'), '" + publishing_year.Text + "', " +
                               "(SELECT id FROM categoryABSU WHERE name = '" + category.Text + "'), " +
                               "(SELECT town FROM publisherABSU WHERE name = '" + publisher.Text + "'), '" + DateTime.Now + "');";
                    }
                    else
                    {
                        query = "INSERT INTO bookABSU (ISBN, title, copies, pages, publisher, publishing_year, category_id, publishung_place, register_date) " +
                               "VALUES ( NEWID(), '" + title.Text + "', '" + copies.Text + "', '" + pages.Text + "', " +
                               "(SELECT id FROM publisherABSU WHERE name = '" + publisher.Text + "'), '" + publishing_year.Text + "', " +
                               "(SELECT id FROM categoryABSU WHERE name = '" + category.Text + "'), " +
                               "(SELECT town FROM publisherABSU WHERE name = '" + publisher.Text + "'), '" + DateTime.Now + "');";
                    }
                    

                }
                else // Изменить
                {
                    query = "UPDATE bookABSU SET " +
                            "title = '" + title.Text + "', " +
                            "copies = '" + copies.Text + "', " +
                            "pages = '" + pages.Text + "', " +
                            "publisher = (SELECT id FROM publisherABSU WHERE name = '" + publisher.Text + "'), " +
                            "publishing_year = '" + publishing_year.Text + "', " +
                            "category_id = (SELECT id FROM categoryABSU WHERE name = '" + category.Text + "') " +
                            "WHERE ISBN = '" + id.Text + "'";
                }
                string enableIdentityInsert = "SET IDENTITY_INSERT bookABSU ON;";
                SqlCommand enableCommand = new SqlCommand(enableIdentityInsert, sqlCon);
                enableCommand.ExecuteNonQuery();
                SqlCommand command = new SqlCommand(query, sqlCon);
                command.ExecuteNonQuery();
                string disableIdentityInsert = "SET IDENTITY_INSERT bookABSU OFF;";
                SqlCommand disableCommand = new SqlCommand(disableIdentityInsert, sqlCon);
                disableCommand.ExecuteNonQuery();

                if (operation=="Добавить") {
                    for (int i = 0; i < int.Parse(copies.Text); i++)
                    {
                        string copyQuery = "INSERT INTO book_copyABSU (book_id, status) " +
                                           "VALUES ('" + id.Text + "', 1);";  // Status 1: В наличии

                        SqlCommand copyCommand = new SqlCommand(copyQuery, sqlCon);
                        copyCommand.ExecuteNonQuery();
                    }
                }


                if (copiesDifference > 0)
                {
                    // Add new book copies
                    for (int i = 0; i < copiesDifference; i++)
                    {
                        string copyQuery = "INSERT INTO book_copyABSU (book_id, status) " +
                                           "VALUES ('" + id.Text + "', 1);";  // Status 1: В наличии

                        SqlCommand copyCommand = new SqlCommand(copyQuery, sqlCon);
                        copyCommand.ExecuteNonQuery();
                    }
                }
                else if (copiesDifference < 0)
                {
                    // Remove existing book copies
                    for (int i = 0; i < Math.Abs(copiesDifference); i++)
                    {
                        string removeIsrQuery = "DELETE TOP (1) FROM book_issuanceABSU WHERE copy IN (SELECT TOP (1) id FROM book_copyABSU WHERE book_id = '" + id.Text + "' AND status = 1);";
                        SqlCommand removeIsCommand = new SqlCommand(removeIsrQuery, sqlCon);
                        removeIsCommand.ExecuteNonQuery();
                        string removeCopyQuery = "DELETE TOP (1) FROM book_copyABSU WHERE book_id = '" + id.Text + "' AND status = 1;";
                        SqlCommand removeCopyCommand = new SqlCommand(removeCopyQuery, sqlCon);
                        removeCopyCommand.ExecuteNonQuery();
                    }
                }
                bool authorsSaved = SaveAuthors(sqlCon);

                if (!authorsSaved && operation == "Добавить")
                {
                    MessageBox.Show("Авторы не были добавлены!");
                    return;
                }
                MessageBox.Show($"{(operation == "Добавить" ? "Книга добавлена" : "Книга обновлена")} успешно!", "Успех");
                OnDataChanged();

                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private int GetCurrentCopiesCount(SqlConnection sqlCon)
        {
            int totalCopies = 0;
            if (operation == "Изменить")
            {
                string query = "SELECT COUNT(*) FROM book_copyABSU WHERE book_id = '" +bookId +"';";
                SqlCommand command = new SqlCommand(query, sqlCon);

                totalCopies = (int)command.ExecuteScalar();
                return totalCopies;
            }
            else return totalCopies;
            
        }
        private void LoadComboBoxes()
        {
            string publisherQuery = "SELECT id, name FROM publisherABSU";
            string categoryQuery = "SELECT id, name FROM categoryABSU";

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    sqlCon.Open();

                    // Загрузка издательств
                    SqlCommand publisherCommand = new SqlCommand(publisherQuery, sqlCon);
                    SqlDataReader publisherReader = publisherCommand.ExecuteReader();
                    publisher.DisplayMemberPath = "Text";
                    publisher.SelectedValuePath = "id";
                    while (publisherReader.Read())
                    {
                        string displayText = publisherReader["name"].ToString();
                        int publID = (int)publisherReader["id"];
                        publisher.Items.Add(new { Text = displayText, id = publID });
                    }

                    publisherReader.Close();

                    // Загрузка категорий
                    SqlCommand categoryCommand = new SqlCommand(categoryQuery, sqlCon);
                    SqlDataReader categoryReader = categoryCommand.ExecuteReader();
                    category.DisplayMemberPath = "Text";
                    category.SelectedValuePath = "id";
                    while (categoryReader.Read())
                    {
                        string displayText = categoryReader["name"].ToString();
                        int catID = (int)categoryReader["id"];
                        category.Items.Add(new { Text = displayText, id = catID });
                    }
                    categoryReader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private bool SaveAuthors(SqlConnection sqlCon)
        {
            bool authorAdded = false;

            foreach (StackPanel panel in AuthorPanel.Children)
            {
                // Поиск TextBox для нового автора
                var newAuthorTextBox = panel.Children
                    .OfType<StackPanel>()
                    .FirstOrDefault(sp => sp.Children.OfType<TextBox>().Any())?
                    .Children.OfType<TextBox>()
                    .FirstOrDefault();

                // Поиск ComboBox для существующего автора
                var existingAuthorComboBox = panel.Children
                    .OfType<StackPanel>()
                    .FirstOrDefault(sp => sp.Children.OfType<ComboBox>().Any())?
                    .Children.OfType<ComboBox>()
                    .FirstOrDefault();

                if (newAuthorTextBox != null && !string.IsNullOrWhiteSpace(newAuthorTextBox.Text))
                {
                    // Новый автор
                    string newAuthorName = newAuthorTextBox.Text;

                    // Проверяем, существует ли автор
                    string authorCheckQuery = "SELECT id FROM authorABSU WHERE full_name = '" + newAuthorName + "'";
                    SqlCommand authorCheckCommand = new SqlCommand(authorCheckQuery, sqlCon);

                    object authorIdObj = authorCheckCommand.ExecuteScalar();
                    int authorId;

                    if (authorIdObj == null) // Автор не существует, добавляем
                    {
                        string insertAuthorQuery = "INSERT INTO authorABSU (full_name) VALUES ('" + newAuthorName + "'); SELECT SCOPE_IDENTITY();";
                        SqlCommand insertAuthorCommand = new SqlCommand(insertAuthorQuery, sqlCon);
                        authorId = Convert.ToInt32(insertAuthorCommand.ExecuteScalar());
                    }
                    else
                    {
                        authorId = Convert.ToInt32(authorIdObj);
                    }

                    // Добавляем связь книга-автор
                    AddAuthorBookLink(sqlCon, authorId);
                    authorAdded = true;
                }
                else if (existingAuthorComboBox != null && existingAuthorComboBox.SelectedValue != null)
                {
                    // Существующий автор
                    int authorId = (int)existingAuthorComboBox.SelectedValue;

                    // Добавляем связь книга-автор
                    AddAuthorBookLink(sqlCon, authorId);
                    authorAdded = true;
                }
            }

            return authorAdded;
        }

        private void AddAuthorBookLink(SqlConnection sqlCon, int authorId)
        {
            string linkCheckQuery = "SELECT COUNT(*) FROM [author&bookABSU] WHERE author = " + authorId + " AND book = '" + id.Text + "'";
            SqlCommand linkCheckCommand = new SqlCommand(linkCheckQuery, sqlCon);

            int linkCount = (int)linkCheckCommand.ExecuteScalar();
            if (linkCount == 0)
            {
                string insertLinkQuery = "INSERT INTO [author&bookABSU] (author, book) VALUES (" + authorId + ", '" + id.Text + "')";
                SqlCommand insertLinkCommand = new SqlCommand(insertLinkQuery, sqlCon);
                insertLinkCommand.ExecuteNonQuery();
            }
        }


        private void Button_Click_AddAuthor(object sender, RoutedEventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(con))
            {
                try
                {
                    sqlCon.Open();

                    string authorQuery = "SELECT id, full_name FROM authorABSU";
                    SqlCommand authorCommand = new SqlCommand(authorQuery, sqlCon);
                    SqlDataReader authorReader = authorCommand.ExecuteReader();

                    // Контейнер для нового набора полей автора
                    StackPanel authorContainer = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(5)
                    };

                    // Строка с Label и TextBox для нового автора
                    StackPanel newAuthorRow = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(5)
                    };
                    Label newAuthorLabel = new Label
                    {
                        Content = "Автор (Нов)",
                        Width = 100,
                        Margin = new Thickness(5)
                    };
                    TextBox newAuthorTextBox = new TextBox
                    {
                        Margin = new Thickness(5),
                        Width = 200
                    };
                    newAuthorRow.Children.Add(newAuthorLabel);
                    newAuthorRow.Children.Add(newAuthorTextBox);

                    // Строка с Label и ComboBox для существующего автора
                    StackPanel existingAuthorRow = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(5)
                    };
                    Label existingAuthorLabel = new Label
                    {
                        Content = "Автор (Сущ)",
                        Width = 100,
                        Margin = new Thickness(5)
                    };
                    ComboBox existingAuthorComboBox = new ComboBox
                    {
                        Margin = new Thickness(5),
                        Width = 200,
                        DisplayMemberPath = "Text",
                        SelectedValuePath = "id"
                    };

                    // Заполнение ComboBox авторами из базы данных
                    while (authorReader.Read())
                    {
                        string authorText = authorReader["full_name"].ToString();
                        int authorID = (int)authorReader["id"];
                        existingAuthorComboBox.Items.Add(new { Text = authorText, id = authorID });
                    }
                    authorReader.Close();

                    existingAuthorRow.Children.Add(existingAuthorLabel);
                    existingAuthorRow.Children.Add(existingAuthorComboBox);

                    // Привязка событий для переключения доступности
                    newAuthorTextBox.TextChanged += (s, ev) =>
                    {
                        existingAuthorComboBox.IsEnabled = string.IsNullOrWhiteSpace(newAuthorTextBox.Text);
                    };

                    existingAuthorComboBox.SelectionChanged += (s, ev) =>
                    {
                        newAuthorTextBox.IsEnabled = existingAuthorComboBox.SelectedItem == null;
                    };

                    // Добавление строк в контейнер
                    authorContainer.Children.Add(newAuthorRow);
                    authorContainer.Children.Add(existingAuthorRow);

                    // Добавление нового автора в основной контейнер
                    AuthorPanel.Children.Add(authorContainer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

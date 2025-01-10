using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для AdminBoard.xaml
    /// </summary>
    public partial class AdminBoard : Window
    {
        public AdminBoard()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Open(new Books());
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Open(new Tickets());
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            Open(new Publishers());
        }

        private void Open(Window window)
        {
            window.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Open(new MainWindow());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Open(new Authors());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Open(new BookNreader());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Open(new BookAcceptance());
        }
    }
}

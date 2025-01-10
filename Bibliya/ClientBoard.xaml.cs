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
    /// Interaction logic for ClientBoard.xaml
    /// </summary>
    public partial class ClientBoard : Window
    {
        public ClientBoard()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Open(new BookReader());
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            Open(new RenewBook());
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

    }
}

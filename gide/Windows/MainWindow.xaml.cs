using gide.Models;
using gide.Pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gide.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Player Player;
        public MainWindow(Player _player)
        {
            InitializeComponent();
            Player = _player;
            MainFrame.Navigate(new CatalogPage(Player));
        }

        private void ToCatalog_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CatalogPage(Player));
        }

        private void ToLibrary_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LibraryPage(Player));
        }
    }
}

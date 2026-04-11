using gide.Models;
using gide.Service;
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

namespace gide.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Btn(object sender, RoutedEventArgs e)
        {
            PlayerService playerService = new();
            var player = playerService.Players.Where(p => p.Username == Username && p.Password == Password);
            if (player.Count() > 0)
            {
                NavigationService.Navigate(new CatalogPage(player.ToArray()[0]));
            }
        }

        private void AuthorLogin_Click(object sender, RoutedEventArgs e)
        {
            AuthorService authorService = new();
            var author = authorService.Authors.Where(a => a.Username == Username && a.Password == Password);
            if (author.Count() > 0)
            {
                NavigationService.Navigate(new AdminPage(author.ToArray()[0]));
            }
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}

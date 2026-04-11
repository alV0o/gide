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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public Player Player = new();
        public Author Author = new();
        public RegistrationPage()
        {
            InitializeComponent();
            DataContext = Player;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Author.Username = Player.Username;
            Author.Password = Player.Password;
            DataContext = Author;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Player.Username = Author.Username;
            Player.Password = Author.Password;
            DataContext = Player;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext == Player)
            {
                PlayerService playerService = new();
                playerService.Add(Player);
            }
            else
            {
                AuthorService authorService = new();
                authorService.Add(Author);
            }
            NavigationService.GoBack();
        }
    }
}

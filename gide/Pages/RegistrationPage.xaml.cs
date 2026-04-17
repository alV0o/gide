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
                if (string.IsNullOrEmpty(Player.Username) ||
                    string.IsNullOrEmpty(Player.Password))
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }
                PlayerService playerService = new();
                if (playerService.Players.Where(p=> p.Username == Player.Username).Count() > 0)
                {
                    MessageBox.Show("Такой пользователь уже существует");
                    return;
                }
                try
                {
                    playerService.Add(Player);
                }
                catch
                {
                    MessageBox.Show("Регистрация неудачна");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Author.Username) ||
                    string.IsNullOrEmpty(Author.Password))
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }
                AuthorService authorService = new();
                if (authorService.Authors.Where(a => a.Username == Author.Username).Count() > 0)
                {
                    MessageBox.Show("Такой автор уже существует");
                    return;
                }
                try
                {
                    authorService.Add(Author);
                }
                catch
                {
                    MessageBox.Show("Регистрация неудачна");
                }
            }
            MessageBox.Show("Регистрация успешна");
            NavigationService.GoBack();
        }
    }
}

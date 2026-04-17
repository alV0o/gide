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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public Author Author { get; set; } = new();
        public Game Game { get; set; } = new();
        public AdminPage(Author _author)
        {
            InitializeComponent();
            Author = _author;
            Game.Author = Author;
            Game.AuthorId = Author.Id;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            GameService gameService = new GameService();
            if (string.IsNullOrEmpty(Game.NameExe) ||
                string.IsNullOrEmpty(Game.Title) ||
                string.IsNullOrEmpty(Game.FullProjectUrl) ||
                string.IsNullOrEmpty(Game.BuildUrl) ||
                string.IsNullOrEmpty(Game.Description))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            try
            {
                gameService.Add(Game);
                MessageBox.Show("Добавление успешно");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Добавление неудачно", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

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
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            GameService gameService = new GameService();
            Game.Author = Author;
            Game.AuthorId = Author.Id;
            gameService.Add(Game);
            MessageBox.Show("Добавление успешно");
            NavigationService.GoBack();
        }
    }
}

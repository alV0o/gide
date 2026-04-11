using gide.Models;
using gide.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        public ObservableCollection<Game> Games { get; set; } = new();
        public Game SelectedGame { get; set; }
        public Player Player { get; set; } = null!;
        GameService _gameService = new();
        public CatalogPage(Player _player)
        {
            InitializeComponent();
            Games = _gameService.Games;
            Player = _player;
            DataContext = this;
        }

        private void Catalog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddToLibrary_Click(object sender, RoutedEventArgs e)
        {
            Player.Games.Add(SelectedGame);
            PlayerService playerService = new PlayerService();
            playerService.Commit();
        }

        private void ToLibrary_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LibraryPage(Player));
        }
    }
}

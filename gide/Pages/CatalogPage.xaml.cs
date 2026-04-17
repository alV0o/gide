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
        public CatalogPage(Player _player)
        {
            InitializeComponent();
            Player = _player;
            Games = new ObservableCollection<Game>(new GameService().Games.Where(g => !Player.Games.Contains(g)));
            DataContext = this;
        }

        private void Catalog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddToLibrary_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGame == null)
            {
                MessageBox.Show("Выберите игру");
                return;
            }
            Player.Games.Add(SelectedGame);
            try
            {
                PlayerService playerService = new PlayerService();
                playerService.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка добавления!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Добавление успешно");
            Games.Remove(SelectedGame);
            SelectedGame = null!;
        }

        private void ToLibrary_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LibraryPage(Player));
        }
    }
}

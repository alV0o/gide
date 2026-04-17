using gide.Models;
using gide.Service;
using gide.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
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

namespace gide.Pages
{
    /// <summary>
    /// Логика взаимодействия для LibraryPage.xaml
    /// </summary>
    public partial class LibraryPage : Page
    {
        private const string DevDir = "dev";
        private const string MainDir = "main";
        private const string GamesDir = "Games";
        private const string ModDir = "modifications";

        private readonly DownloadService _downloadService = new();
        public ObservableCollection<Game> Games { get; set; }
        public Game SelectedGame { get; set; } = null!;
        public Player Player { get; set; }
        public LibraryPage(Player _player)
        {
            InitializeComponent();
            Player = _player;
            Games = new ObservableCollection<Game>(Player.Games);
            DataContext = this;
        }

        private void ToLibrary_Click(object sender, RoutedEventArgs e)
        {

        }

        private async Task UnpackGameAsync(string archivePath, string folderPath)
        {
            try
            {
                await Task.Run(() =>
                {
                    ZipFile.ExtractToDirectory(archivePath, folderPath);

                    File.Delete(archivePath);

                    if (Directory.GetDirectories(folderPath).Length > 0)
                    {
                        string di = Directory.GetDirectories(folderPath)[0];

                        foreach (var file in Directory.GetFiles(di))
                        {
                            string filePath = Path.Combine(folderPath, Path.GetFileName(file));
                            File.Move(file, filePath);
                        }

                        foreach (var dir in Directory.GetDirectories(di))
                        {
                            string directoryPath = Path.Combine(folderPath, Path.GetFileName(dir));
                            Directory.Move(dir, directoryPath);
                        }

                        Directory.Delete(di);
                    }
                });
            }
            catch
            {
                MessageBox.Show("Ошибка распаковки");
            }
        }

        private async void Download(Game game, string folder)
        {
            string folderPath = Path.Combine(GamesDir, game.Title, folder);
            if (Directory.Exists(folderPath))
            {
                if (folder == DevDir) MessageBox.Show("Проект скачан");
                else if (folder == MainDir) MessageBox.Show("Игра скачана");
                else MessageBox.Show("Unknown folder");
                return;
            }
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, $"{folder}.zip");
            try
            {
                await _downloadService.DownloadFileAsync(game.FullProjectUrl, filePath);
                await UnpackGameAsync(filePath, folderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                Directory.Delete(Path.Combine(GamesDir, game.Title), true);
                return;
            }


            MessageBox.Show("Загрузка и распаковка завершены!");
        }


        private void Download_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGame == null)
            {
                MessageBox.Show("Выберите игру");
                return;
            }
            Download(SelectedGame, MainDir);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGame == null)
            {
                MessageBox.Show("Выберите игру");
                return;
            }
            string folderPath = Path.Combine(GamesDir, SelectedGame.Title, MainDir);
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Скачайте игру");
                return;
            }

            DirectoryInfo di = new DirectoryInfo(folderPath);

            FileInfo[] file = di.GetFiles(SelectedGame.NameExe, SearchOption.AllDirectories);
            if (file.Length > 0) Process.Start(file.First().FullName);
            else
            {
                MessageBox.Show("Отсутствует запускаемый файл");
                return;
            }
        }

        private void ToCatalog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DownloadFullProject_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGame == null)
            {
                MessageBox.Show("Выберите игру");
                return;
            }
            Download(SelectedGame, DevDir);
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGame == null)
            {
                MessageBox.Show("Выберите игру");
                return;
            }
            string folderPath = Path.Combine(GamesDir, SelectedGame.Title, DevDir);
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Скачайте проект");
                return;
            }
            if (new DirectoryInfo(folderPath).GetDirectories().Length > 0 || new DirectoryInfo(folderPath).GetFiles("*", SearchOption.AllDirectories).Length > 0) NavigationService.Navigate(new ProjectPage(new DirectoryInfo(folderPath), SelectedGame.NameExe));
            
        }

        private void ViewMod_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedGame == null)
            {
                MessageBox.Show("Выберите игру");
                return;
            }
            string folderPath = Path.Combine(GamesDir, SelectedGame.Title, ModDir);
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Моды отсутствуют");
                return;
            }
            ViewModModalWindow viewModModalWindow = new(new DirectoryInfo(folderPath));
            if (viewModModalWindow.ShowDialog() == true) 
            {
                NavigationService.Navigate(new ProjectPage(new DirectoryInfo(Path.Combine(folderPath, viewModModalWindow.SelectedMod)), SelectedGame.NameExe ,true));
            }
        }
    }
}

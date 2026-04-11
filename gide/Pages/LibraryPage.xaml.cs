using gide.Models;
using gide.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;

namespace gide.Pages
{
    /// <summary>
    /// Логика взаимодействия для LibraryPage.xaml
    /// </summary>
    public partial class LibraryPage : Page
    {
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
            await Task.Run(() =>
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(archivePath, folderPath);

                if (File.Exists(archivePath))
                {
                    File.Delete(archivePath);
                }

                if(Directory.GetDirectories(folderPath).Length > 0)
                {
                    string di = Directory.GetDirectories(folderPath)[0];

                    foreach ( var file in Directory.GetFiles(di))
                    {
                        string filePath = System.IO.Path.Combine(folderPath, System.IO.Path.GetFileName(file));
                        File.Move(file, filePath);
                    }

                    foreach(var dir in Directory.GetDirectories(di))
                    {
                        string directoryPath = System.IO.Path.Combine(folderPath, System.IO.Path.GetFileName(dir));
                        Directory.Move(dir, directoryPath);
                    }

                    Directory.Delete(di);
                }
            });
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = System.IO.Path.Combine("game", SelectedGame.Title);
            Directory.CreateDirectory(folderPath);

            string filePath = System.IO.Path.Combine(folderPath, "game.zip");

            await _downloadService.DownloadFileAsync(SelectedGame.BuildUrl, filePath);

            await UnpackGameAsync(filePath, folderPath);

            MessageBox.Show("Загрузка и распаковка завершены!");
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = System.IO.Path.Combine("game", SelectedGame.Title);

            DirectoryInfo di = new DirectoryInfo(folderPath);

            FileInfo[] file = di.GetFiles(SelectedGame.NameExe, SearchOption.AllDirectories);

            Process.Start(file[0].FullName);
        }

        private void ToCatalog_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void DownloadFullProject_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = System.IO.Path.Combine("dev", SelectedGame.Title);
            Directory.CreateDirectory(folderPath);

            string filePath = System.IO.Path.Combine(folderPath, "dev.zip");

            await _downloadService.DownloadFileAsync(SelectedGame.FullProjectUrl, filePath);

            await UnpackGameAsync(filePath, folderPath);

            MessageBox.Show("Загрузка и распаковка завершены!");
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = System.IO.Path.Combine("dev", SelectedGame.Title);

            NavigationService.Navigate(new ProjectPage(new DirectoryInfo(folderPath)));
        }
    }
}

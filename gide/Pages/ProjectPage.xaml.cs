using gide.Models;
using gide.Service;
using gide.Windows;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace gide.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public ObservableCollection<DirectoryClass> MainDirectory { get; set; } = new();
        public TextDocument TextEdit { get; set; } = new();
        private FileClass fileClass;

        private bool _isModded = false;
        public bool IsModded 
        { 
            get=>_isModded; 
            set 
            { 
                _isModded = value;
                OnPropertyChanged("IsModded");
            } 
        }

        private string gameName = null!;


        public ProjectPage(DirectoryInfo _di, string _gameName, bool? _isMod = null)
        {
            InitializeComponent();
            if (_isMod == true) IsModded = true;
            MainDirectory.Add(AddToList(_di));
            gameName = _gameName;
            avalonedit.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
        }



        public DirectoryClass AddToList(DirectoryInfo mainDir)
        {
            DirectoryClass newDir = new DirectoryClass(mainDir.Name, mainDir.FullName);

            ObservableCollection<FileBaseItem> children = new();
            foreach (var dr in mainDir.GetDirectories())
            {
                children.Add(AddToList(dr));
            }

            foreach (var fl in mainDir.GetFiles())
            {
                children.Add(new FileClass(fl.Name, fl.FullName));
            }

            newDir.Children = children;
            return newDir;
        }

        private void treeview1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView treeView = sender as TreeView;
            if (treeView != null)
            {
                var selectedItem = treeView.SelectedItem;

                if (selectedItem is FileClass)
                {
                    FileClass temp = (FileClass)selectedItem;
                    TextEdit.Text = File.ReadAllText(temp.Path);
                    fileClass = temp;
                }
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(fileClass.Path, TextEdit.Text);
            MessageBox.Show("Сохранено");
        }

        private async void BuildBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] path = Directory.GetFiles(MainDirectory[0].Path, "*.csproj", SearchOption.AllDirectories);

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{path[0]}\"",
                RedirectStandardOutput = false,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (var process = Process.Start(startInfo))
            {
                await process.WaitForExitAsync();
                MessageBox.Show("Сборка прошла успешно!");
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            string[] folderPath = Directory.GetFiles(MainDirectory[0].Path, gameName, SearchOption.AllDirectories);
            string lastUpdatedPath = folderPath[0];

            foreach(string folderPathItem in folderPath)
            {
                if (File.GetCreationTime(folderPathItem) > File.GetCreationTime(lastUpdatedPath)) lastUpdatedPath = folderPathItem;
            }

            Process.Start(lastUpdatedPath);
        }

        //https://learn.microsoft.com/ru-ru/dotnet/standard/io/how-to-copy-directories
        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            var dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destinationDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }



        private void AddMod_Click(object sender, RoutedEventArgs e)
        {

            SaveModModalWindow saveModModalWindow = new SaveModModalWindow();
            if (saveModModalWindow.ShowDialog() == true)
            {
                string projectPath = MainDirectory[0].Path.Remove(MainDirectory[0].Path.LastIndexOf('\\'));
                string folderPath = Path.Combine(projectPath, "modifications", saveModModalWindow.Title);
                Directory.CreateDirectory(folderPath);

                CopyDirectory(MainDirectory[0].Path, folderPath, true);
                MainDirectory[0] = AddToList(new DirectoryInfo(folderPath));

                IsModded = true;
            }
        }
    }
}

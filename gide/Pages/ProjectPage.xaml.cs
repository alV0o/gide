using gide.Models;
using gide.Service;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
    /// Логика взаимодействия для ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Page
    {
        public ObservableCollection<DirectoryClass> MainDirectory { get; set; } = new();
        public TextDocument TextEdit { get; set; } = new();
        private FileClass fileClass;
        public ProjectPage(DirectoryInfo _di)
        {
            InitializeComponent();
            MainDirectory.Add(AddToList(_di));
            avalonedit.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C#");
        }

        public DirectoryClass AddToList(DirectoryInfo mainDir)
        {
            DirectoryClass newDir = new DirectoryClass(mainDir.Name, mainDir.FullName);

            ObservableCollection<DirectoryClass> dirClasses = new ObservableCollection<DirectoryClass>();
            foreach (var dr in mainDir.GetDirectories())
            {
                dirClasses.Add(AddToList(dr));
            }

            ObservableCollection<FileClass> fileClasses = new ObservableCollection<FileClass>();
            foreach (var fl in mainDir.GetFiles())
            {
                fileClasses.Add(new FileClass(fl.Name, fl.FullName));
            }

            newDir.Files = fileClasses;
            newDir.Directories = dirClasses;
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
            string nameExe = new GameService().Games.Where(g => g.Title == MainDirectory[0].Name).FirstOrDefault().NameExe;
            string[] folderPath = Directory.GetFiles(MainDirectory[0].Path, nameExe, SearchOption.AllDirectories);
            string lastUpdatedPath = folderPath[0];

            foreach(string folderPathItem in folderPath)
            {
                if (File.GetCreationTime(folderPathItem) > File.GetCreationTime(lastUpdatedPath)) lastUpdatedPath = folderPathItem;
            }

            Process.Start(lastUpdatedPath);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace gide.Windows
{
    /// <summary>
    /// Логика взаимодействия для ViewModModalWindow.xaml
    /// </summary>
    public partial class ViewModModalWindow : Window
    {
        public ObservableCollection<string> Mods { get; set; } = new();
        public string SelectedMod { get; set; } = string.Empty;
        
        public ViewModModalWindow(DirectoryInfo _di)
        {
            InitializeComponent();
            Mods = new ObservableCollection<string>(_di.GetDirectories().Select(d=>d.Name));
            DataContext = this;
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}

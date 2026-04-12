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
using System.Windows.Shapes;

namespace gide.Windows
{
    /// <summary>
    /// Логика взаимодействия для SaveModModalWindow.xaml
    /// </summary>
    public partial class SaveModModalWindow : Window
    {
        string Title { get; set; } = null!;
        public SaveModModalWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gide.Models
{
    public class DirectoryClass
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public ObservableCollection<FileClass> Files { get; set; } = new();
        public ObservableCollection<DirectoryClass> Directories { get; set; } = new();
        public DirectoryClass(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}

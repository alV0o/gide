using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gide.Models
{
    public class DirectoryClass : FileBaseItem
    {
        public ObservableCollection<FileBaseItem> Children { get; set; } = new();
        public DirectoryClass(string name, string path) : base(name, path) { }
    }
}

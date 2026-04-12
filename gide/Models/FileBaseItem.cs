using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gide.Models
{
    public abstract class FileBaseItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        protected FileBaseItem(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}

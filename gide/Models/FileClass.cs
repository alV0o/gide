using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gide.Models
{
    public class FileClass
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public FileClass(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}

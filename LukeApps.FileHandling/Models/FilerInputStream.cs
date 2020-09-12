using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeApps.FileHandling.Models
{
    class FilerInputStream
    {
        public string FileName { get; set; }
        public string FileCategoryKey { get; set; }
        public string ContentType { get; set; }
        public int ContentLength { get; set; }
        public Stream FInputStream { get; set; }
    }
}

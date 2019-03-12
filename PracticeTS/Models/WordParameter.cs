using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PracticeTS.Models
{
    public class WordParameter
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public FileInfo Image { get; set; }
    }
}
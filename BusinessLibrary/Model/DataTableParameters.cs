using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLibrary.Model
{
    public class DataTableParameters
    {
        public int start { get; set; } = 0;
        public int length { get; set; } = 0;
        public string orderBy { get; set; } = "";
        public string order { get; set; } = "asc";
        public string filter { get; set; } = "";
    }
}

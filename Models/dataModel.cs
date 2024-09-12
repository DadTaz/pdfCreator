using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdfCreator.Models
{

    public class dataModel
    {
        public string? Title { get; set; }
        public string? NormalText { get; set; }
        public string? BoldText { get; set; }
        public string? ItalicText { get; set; }
        public string[][]? TableDataFull { get; set; }
        public string[][]? TableDataPartial { get; set; }
    }
}

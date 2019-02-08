using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OXGame.Models
{
    public class TableRows
    {
        public int FirstColumn { get; set; }
        public int SecondColumn { get; set; }
        public int ThirdColumn { get; set; }

        public string[] FirstRow { get; set; }
        public string[] SecondRow { get; set; }
        public string[] ThirdRow { get; set; }
        public string CellId { get; set; }
    }
}
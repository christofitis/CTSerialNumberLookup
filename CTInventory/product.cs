using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CTInventory
{
    class product
    {
        public string name { get; set; }
        public string sheet { get; set; }
        public string prefix { get; set; }
        public int number { get; set; }
        public int numberLow { get; set; }

        public string userInputSerialNumber { get; set; }

        public string serialNumberLow { get; set; }
        public string serialNumberHigh { get; set; }

        public string notesLow { get; set; }
        public string notesHigh { get; set; }

        public string revisionHigh { get; set; }
        public string revisionLow { get; set; }


        public string purchaseDateLow { get; set; }
        public string purchaseDateHigh { get; set; }

        public DateTime dateTimeLow { get; set; }
        public DateTime dateTimeHigh { get; set; }

    }
}

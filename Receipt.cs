using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp17
{
    class Receipt
    {
        public int Id { get; set; }
        public string ItemsName { get; set; }
        public double Prices { get; set; }
        public int Quantity { get; set; }
        public string Total { get { return string.Format("{0}$", Prices * Quantity); } }
    }
}

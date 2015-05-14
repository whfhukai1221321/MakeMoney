using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeMoney
{
    public class Stock
    {
        public DateTime Date { get; set; }

        public float Open { get; set; }

        public float High { get; set; }

        public float Low { get; set; }

        public float Close { get; set; }

        public long Volume { get; set; }

        public float AdjClose { get; set; }
    }
}

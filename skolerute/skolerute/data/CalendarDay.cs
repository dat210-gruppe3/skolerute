using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skolerute.data
{
    class CalendarDay
    {
        public int ID { get; set; }
        public DateTime date { get; set; }
        public bool isFreeDay { get; set; }
        public bool isNotWorkDay { get; set; }
        public bool isSFODay { get; set; }
        public string comment { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skolerute.data
{
    class SchoolList
    {

        public IEnumerable<string> liste { get; set; }

        public SchoolList(IEnumerable<string> liste)
        {
            this.liste = liste;
        }

        public SchoolList()
        {
            this.liste = null;
        }
}
}

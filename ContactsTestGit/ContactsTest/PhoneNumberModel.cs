using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsTest
{
    public class Phones
    {
        public int id { get; set; }

        public int id_fk { get; set; }

        public string number { get; set; }

        public int phoneType { get; set; }

        public MainData data { get; set; }
    }
}

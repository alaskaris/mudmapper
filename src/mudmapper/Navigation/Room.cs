using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mudmapper.Navigation
{
    class Room
    {
        public string name { get; set; }
        public string exits { get; set; }
        public int id { get; set; }
        public Room north { get; set; }
        public Room south { get; set; }
        public Room east { get; set; }
        public Room west { get; set; }
        public Room up { get; set; }
        public Room down { get; set; }
    }
}

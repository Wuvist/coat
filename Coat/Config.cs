using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coat
{
    class Config
    {
        public string Conn { get; set; }
        public string Namespace { get; set; }
        public string Output { get; set; }
        public List<string> Tables { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using d2d;

namespace Coat.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var o = d2d.AdminInfo.FindOne("UserName = 'testuser'", null);
            Console.WriteLine(o.UserName);

        }
    }
}

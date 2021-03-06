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
            var user = d2d.AdminInfo.FindOne(p => (p.UserName == "admin"));
            Console.WriteLine(user.Password);

            var objs = d2d.AdminInfo.Find(p => (p.IsValid == true), 2, 0);
            foreach (var o in objs)
            {
                Console.WriteLine(o.UserName);
            }
            Console.ReadLine();
        }
    }
}

﻿using System;
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
            d2d.AdminInfo.Find(p => (p.UserName == ""));

            var objs = d2d.AdminInfo.GetByIDs(new List<int>(){1, 2});
            foreach (var o in objs) {
                Console.WriteLine(o.UserName);

            }
            
        }
    }
}

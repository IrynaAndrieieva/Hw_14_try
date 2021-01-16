using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw_14_try
{
    class Program
    {
        static void Main(string[] args)
        {
            _ = new Starter();
            var bl = new BusinessLogic();
            bl.Run();
            Console.ReadLine();
        }
    }
}

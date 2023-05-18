using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IQTR;

namespace IQTR.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ClassLibrary.MyCode obj = new ClassLibrary.MyCode();
            obj.DoSomething();
            Console.ReadLine();
        }
    }
}

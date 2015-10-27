using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ASCII_house
{
    class Program
    {
        static void Main(string[] args)
        {
            Construction construction = new Construction("Sample3.txt");

            construction.startConstruction();

            construction.print();

        }
    }
}

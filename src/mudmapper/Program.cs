using mudmapper.Utils;
using System;

namespace mudmapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Parser p = new Parser();
            p.file = @"D:\Users\alask\OneDrive\Games\mud\mud logs\1.txt";
            p.parse();

        }
    }
}

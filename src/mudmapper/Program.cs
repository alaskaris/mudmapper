using mudmapper.Utils;
using System;

namespace mudmapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser p = new Parser();
            //p.file = @"C:\Users\alask\OneDrive\Games\mud\mud logs\1.txt";
            //p.parse();
            p.chunkFile = @"1_gyskbgqh.jes.chnk";
            p.process();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Project2
{

    class MainClass
    {
        public static void Main(string[] args)
        {
            switch(args[0].ToUpper())
            {
                case "GRAPH1":
                    AutonomousSystems autoSystems = new AutonomousSystems(args[1]);
                    break;
                case "GRAPH2":
                    //IPRange r = new IPRange("216.88.0.0", 14);
                    //Console.WriteLine("Start: {0}, End: {1}", r.start.ToString(), r.end.ToString());
                    SubSection2 g = new SubSection2(args[1], args[2]);      
                    //g.printAll();
                    g.ExportGraph4Data();
                    //g.ExportGraph2Data();
                    break;
                case "GRAPH3":
                    //stuff
                    break;
                case "GRAPH4":
                    //stuff
                    break;
                case "TABLE1":
                    //stuff
                    break;
                case "TABLE2":
                    //stuff
                    break;
                case "TABLE3":
                    //stuff
                    break;
            }

            //TEST for git
        }
    }
}

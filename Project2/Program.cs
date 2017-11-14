﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Project2
{

    class MainClass
    {
        public static void Main(string[] args)
        {
            SubSection2 g;

            switch (args[0].ToUpper())
            {
                case "GRAPH1":
                    AutonomousSystems autoSystems = new AutonomousSystems(args[1]);
                    break;
                case "GRAPH2":
                    g = new SubSection2(args[1], args[2]);
                    g.ExportGraph2Data();
                    break;
                case "GRAPH3":
                    g = new SubSection2(args[1], args[2]);
                    g.ExportGraph3Data();
                    break;
                case "GRAPH4":
                    g = new SubSection2(args[1], args[2]);
                    g.ExportGraph4Data();
                    break;
                case "TABLE1":
                    g = new SubSection2(args[1], args[2]);
                    g.ExportTable1Data();
                    break;
                case "TABLE2":
                    g = new SubSection2(args[1], args[2]);
                    g.ExportTable2Data();
                    break;
                case "TABLE3":
                    g = new SubSection2(args[1], args[2]);
                    g.ExportTable2Data();
                    break;
            }

            //TEST for git
        }
    }
}

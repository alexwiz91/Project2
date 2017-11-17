﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Project2
{

    class MainClass
    {
        public static void Main(string[] args)
        {
            AutonomousSystems autoSystems = null;
            SubSection2 g;

            switch (args[0].ToUpper())
            {
                case "GRAPH1":
                    autoSystems = new AutonomousSystems(args[1]);
                    autoSystems.ExportGraph1Data();
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
                default:
                    autoSystems = new AutonomousSystems(args[1]);
                    autoSystems.ExportGraph1Data();

                    g = new SubSection2(args[1], args[2]);
                    g.ExportGraph2Data();
                    g.ExportGraph3Data();
                    g.ExportGraph4Data();
                    g.ExportTable1Data();
                    g.ExportTable2Data();
                    break;
            }

            //TEST for git
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    enum ASTYPE { ENTERPRISE, CONTENT, TRANSIT, UNKNOWN };

    class AutonomousSystem
    {
        public ASTYPE type;

        public AutonomousSystem()
        {
            type = ASTYPE.UNKNOWN;
        }

        public AutonomousSystem(string line)
        {
            if (line.ToUpper().Contains("CONTENT"))
                type = ASTYPE.CONTENT;
            else if (line.ToUpper().Contains("ENTERPRISE"))
                type = ASTYPE.ENTERPRISE;
            else if (line.ToUpper().Contains("TRANSIT"))
                type = ASTYPE.TRANSIT;
        }
    }

    class AutonomousSystems : List<AutonomousSystem>
    {
        public AutonomousSystems() : base() { }
        public AutonomousSystems(string filename) : base()
        {
            AutonomousSystem autoSys = new AutonomousSystem();

            foreach (string line in File.ReadAllLines(filename))
            {
                if (line[0] == '#')
                    continue;

                Add(new AutonomousSystem(line));
            }
        }

        public int countType(ASTYPE type)
        {
            int count = 0;

            foreach (AutonomousSystem autoSys in this)
            {
                if (autoSys.type == type)
                    count++;
            }

            return count;
        }
    }
}

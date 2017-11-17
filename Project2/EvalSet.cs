using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public sealed class EvalSet : HashSet<string>
    {
        private static EvalSet instance = null;
        private static readonly object padlock = new object();
        public uint totalIpSpace = 0;
        public int totalPrefix = 0;

        private EvalSet() : base()
        {
        }

        public static EvalSet Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new EvalSet();
                    }
                    return instance;
                }
            }
        }
    }
}

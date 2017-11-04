using System;
using System.Collections.Generic;
using System.IO;

namespace Project2
{
    public class Graph2 :  Dictionary<string, HashSet<string>>
    {
        
        public Graph2(string filename) : base()
        {

            string[] split_member = new string[4];
            foreach (string line in File.ReadAllLines(filename))
            {
                if (line[0] == '#')
                    continue;
                split_member = line.Split('|');
                if (split_member.Length > 1)
                {
                    if (!this.ContainsKey(split_member[0]))
                    {
                        Add(split_member[0], new HashSet<string>());
                    }
                    this[split_member[0]].Add(split_member[1]);
                }

            }
        }

        public void printAll()
        {
            foreach(KeyValuePair<string, HashSet<string>> kv in this)
            {
                Console.Write("Key: {0}, Values: ", kv.Key);
                foreach(string str in kv.Value)
                {
                    Console.Write(", {0}", str);
                }
                Console.WriteLine();
            }


        }
    }
}

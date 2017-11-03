using System;
using System.Collections.Generic;
using System.IO;

namespace Project2
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            if (args.Length == 1)
            {
                int CountASEnterprise = 0;
                int CountASContent = 0;
                int CountASTA = 0;
                int CountBadInput = 0;
                Console.WriteLine(args.Length);
                string[] all_lines = System.IO.File.ReadAllLines(args[0]);
                List<String> all_lines_filtered = new List<string>();
                foreach (string line in all_lines)
                {
                    if (!(line[0] == '#'))
                        all_lines_filtered.Add(line);
                }

                foreach (string filtered_line in all_lines_filtered)
                {
                    if (filtered_line.ToUpper().Contains("CONTENT"))
                        CountASContent++;
                    else if (filtered_line.ToUpper().Contains("ENTERPISE"))
                        CountASEnterprise++;
                    else if (filtered_line.ToUpper().Contains("TRANSIT"))
                        CountASTA++;
                    else
                        CountBadInput++;
                }

                Console.WriteLine("Number of Transit/Access: {0}", CountASTA);
                Console.WriteLine("Number of Content: {0}", CountASContent);
                Console.WriteLine("Number of Enterprise: {0}", CountASEnterprise);
                Console.WriteLine("Number of Bad Input: {0}", CountBadInput);



                var sw = new StreamWriter("./output.csv");
                sw.WriteLine("Number of Transit/Access,Number Of Content, Number of Enterprise, Number of Bad Input");
                sw.WriteLine(string.Format("{0},{1},{2},{3}", CountASTA, CountASContent, CountASEnterprise, CountBadInput));
                sw.Flush();
                sw.Close();
                
            }
        }
    }
}

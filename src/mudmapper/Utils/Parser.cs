using mudmapper.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mudmapper.Utils
{
    class Parser
    {
        public string file { get; set; }

        public void parse()
        {
            if (!File.Exists(file))
                throw new ArgumentException("file cannot be found");

            using (StreamReader filestream = new StreamReader(file))
            {
                string currentLine = null;
                //Room currentRoom = null;

                string chunk = String.Empty;
                bool addToChunk = false;

                while ((currentLine = filestream.ReadLine()) != null)
                {
                    if (currentLine.Contains("Exits:"))
                    {
                        chunk += currentLine;
                        addToChunk = true;
                    }

                    if (currentLine == "\n")
                    {
                        addToChunk = false;
                        process(chunk);
                    }

                    if (addToChunk)
                        chunk += currentLine;

                }
            }


        }

        private void process(string chunk)
        {
            Debug.WriteLine($"chunk is--> {chunk}");
            var input = String.Empty;
            Regex soloPattern = new Regex(@"Exits:[NESWUD]+>.\w*\r\n.*");
            Regex groupPattern = new Regex(@"Exits:[NESWUD]+>\s\w*\r\n.*\r\nYou follow \w* (south|north|east|west|up|down).\r\n.*\r\n");

            if (soloPattern.IsMatch(chunk))
            {
                input = soloPattern.Match(chunk).Value;
                Debug.WriteLine($"matched solo--> {input}");
            }

            if (groupPattern.IsMatch(chunk))
            {
                input = groupPattern.Match(chunk).Value;
                Debug.WriteLine($"matched group--> {input}");
            }

            //Exits:[NESWUD]+> --> exits
            //\r\n.* --> direction
            //\w\r\n --> description


        }
    }
}

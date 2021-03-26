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
        public string chunkFile { get; set; }
        public string finalFile { get; private set; }

        public void parse()
        {
            if (!File.Exists(file))
                throw new ArgumentException("file cannot be found");

            chunkFile = $"{Path.GetFileNameWithoutExtension(file)}_{Path.GetRandomFileName()}.chnk";

            using (StreamReader filestream = new StreamReader(file))
            using (StreamWriter chunkStream = new StreamWriter(chunkFile))
            {
                string currentLine = null;
                string chunk = String.Empty;

                while ((currentLine = filestream.ReadLine()) != null)
                {
                    Debug.WriteLine($"line--> {currentLine}");
                    chunk += "|" + currentLine;
                    if (currentLine.Contains("Exits:"))
                    {
                        Debug.WriteLine($"chnk--> {chunk}");
                        chunkStream.WriteLine(chunk);
                        chunk = String.Empty;
                    }
                }
            }
        }

        public void process()
        {
            finalFile = $"{Path.GetFileNameWithoutExtension(file)}_proper.txt";
            using (StreamReader chunkStream = new StreamReader(chunkFile))
            using (StreamWriter finalStream = new StreamWriter(finalFile))
            {
                string currentLine = null;
                bool hasMisplacedDescription = false;
                string misplacedDescription = String.Empty;
                bool hasMisplacedCommand = false;
                string misplacedCommand = String.Empty;
                string description = String.Empty;
                while ((currentLine = chunkStream.ReadLine()) != null)
                {
                    Debug.WriteLine($"line--> {currentLine}");
                    var currentArray = currentLine.Split("|");
                    // first item is empty
                    var firstItem = currentArray.Length > 0 ? currentArray[0] : String.Empty;
                    // check if second item is a command. if yes, then it is a misplaced one.
                    var secondItem = currentArray.Length > 1 ? currentArray[1] : String.Empty;
                    // description is either the second, the third in the case of a misplaced command
                    // or there is none when the previous line didn't contain any command
                    var thirdItem = currentArray.Length > 2 ? currentArray[2] : String.Empty;
                    // fix placement and keep the text after the directions to a buffer
                    var lastItem = currentArray.Last();
                    // the last item contains the directions and the command if any
                    var (exits, command) = lastItem.getExitsCommand();

                    // if the previous one was misplaced then the description is passed down
                    if (hasMisplacedDescription)
                    {
                        description = misplacedDescription;
                        hasMisplacedDescription = false;
                    }

                    if (hasMisplacedCommand)
                    {
                        hasMisplacedDescription = true;
                        misplacedDescription = command;
                        command = misplacedCommand;
                        description = secondItem;
                        hasMisplacedCommand = false;
                    }

                    // usual case: second item is description, last item is exits + command
                    if (!String.IsNullOrEmpty(command) && command.isCommand())
                    {
                        description = secondItem;
                        finalStream.WriteLine($"{description}|{exits}|{command}");
                    }

                    // if there is a misplaced command with an actual command then
                    // the next line contains two descriptions: second and command
                    if (!String.IsNullOrEmpty(command) && command.isCommand() && secondItem.isCommand())
                    {
                        finalStream.WriteLine($"{description}|{exits}|{command}");
                    }

                    // if there is a misplaced command, the second item is the command
                    // and what exists after the exits is the description of the next room
                    if (!String.IsNullOrEmpty(command) && !command.isCommand() && secondItem.isCommand())
                    {
                        hasMisplacedDescription = true;
                        misplacedDescription = command;
                        description = thirdItem;
                        command = secondItem;
                        finalStream.WriteLine($"{description}|{exits}|{command}");
                    }

                    
                    // if there is no command, the next line is the same room
                }
            }
            //Regex soloPattern = new Regex(@"Exits:[NESWUD]+>.\w*|.*");
            //Regex groupPattern = new Regex(@"Exits:[NESWUD]+>\s\w*|.*\r\nYou follow \w* (south|north|east|west|up|down).|.*");
            //Exits:[NESWUD]+> --> exits
        }
    }
}

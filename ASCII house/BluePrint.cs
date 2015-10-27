using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ASCII_house
{
    //  This class is used for creating a blueprint layout of the structure
    class BluePrint
    {
        // Stores the variable recieved from the file
        private int totalNumberofLines;

        // Stores the asterick lines from the file 
        private String[] linesInBluePrint;

        // Stores the max chars on one line
        private int widthInBluePrint;

        // totalNumberoflines and height are same
        private int heightInBluePrint;

        // Stores the coordinates object created using the file
        private Coordinates blueprintCoordinates;

        // Checks whether object construction was successful or not
        private bool wasBluePrintConstructionSuccessful;

        public BluePrint(String filename)
        {
            try
            {
                String line;

                // Reads the file
                StreamReader sr = new StreamReader(filename);

                // Get first line
                line = sr.ReadLine();

                // Convert first line to int
                totalNumberofLines = Int32.Parse(line);

                
                linesInBluePrint = new String[totalNumberofLines];

                int count = -1;

                while (line != null && count < totalNumberofLines)
                {
                    // Goes in everytime except for the first time
                    if (count != -1)
                    {

                        linesInBluePrint[count] = line;

                        line = sr.ReadLine();
                    }
                    else
                    {
                        line = sr.ReadLine();
                    }
                    ++count;
                }

                sr.Close();

                //Initializing rest of the variables
                heightInBluePrint = totalNumberofLines;
                widthInBluePrint = linesInBluePrint[totalNumberofLines - 1].Length;
                blueprintCoordinates = new Coordinates(widthInBluePrint + 1, heightInBluePrint + 1);

                // If there is blank in file value is -1 , if there is an asterick value is 1 in coordinates
                for (int runY = 1; runY < heightInBluePrint + 1; runY++)
                {
                    String currentLine = linesInBluePrint[runY - 1];

                    for (int runX = 1; runX < widthInBluePrint + 1; runX++)
                    {
                        if (runX - 1 < currentLine.Length)
                        {
                            if (currentLine.Substring(runX - 1, 1) == "*")
                                blueprintCoordinates.addCoordinates(runX, runY, 1);
                            else
                                blueprintCoordinates.addCoordinates(runX, runY, -1);

                        }
                        else
                            blueprintCoordinates.addCoordinates(runX, runY, -1);

                        
                    }
                    
                }

                wasBluePrintConstructionSuccessful = true;

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                wasBluePrintConstructionSuccessful = false;
            }
        }

        public bool wasBluePrintConstrunctionSuccessful()
        {
            return this.wasBluePrintConstructionSuccessful;
        }

        public int getWidthInBluePrint()
        {
            return this.widthInBluePrint;
        }

        public int getHeightInBluePrint()
        {
            return this.heightInBluePrint;
        }

        public String getCurrentLine(int lineNumber)
        {
            if (lineNumber >= 0)
                return this.linesInBluePrint[lineNumber];
            else
                return null;
        }

        public Coordinates getCoordinates()
        {
            return this.blueprintCoordinates;
        }
    }


}

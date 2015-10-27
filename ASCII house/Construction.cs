using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ASCII_house
{
    
    // The class is used for construction of the house
    class Construction
    {
        // Stores the asterick blueprint 
        private BluePrint blueprint;
        
        // The building blocks of the house
        private string roofApex;
        private string roofLeft;
        private string roofRight;
        private string Edge;
        private string Brick;
        private string sideWall;
        private string window;
        
        // Maximum Height and Width of a House
        private int maximumHeight;
        private int maximumWidth;

        // Will be used to built walls
        private int currentYCoordinate;

        // Store possible location for doors , eligible location of windows and the chosen window location
        private List<Tuple<int,int>> locationofWindows;
        private List<Tuple<int, int>> possibleDoorLocations;
        private Tuple<int, int> finalDoorLocation;

        
        // The array that will store the House
        private String[,] House;

        public Construction(String filename)
        {
            // Creates the blueprint
            this.blueprint = new BluePrint(filename);

            // Initializing the builting blocks
            this.roofApex = "A";
            this.roofLeft = "/";
            this.roofRight= "\\";
            this.Edge = "+";
            this.Brick = "-";
            this.sideWall = "|";
            this.window = "o";
            

            // If only blueprint construction was successful
            if (this.blueprint.wasBluePrintConstrunctionSuccessful())
            {

                // The dimensions are scaled according to the blueprints
                this.maximumWidth = 5 * this.blueprint.getWidthInBluePrint();       
                this.maximumHeight = 2 * this.blueprint.getHeightInBluePrint() + this.maximumWidth;
                this.House = new String[this.maximumWidth, maximumHeight];

                // array is filled with blank spaces
                for (int runY = 0; runY < maximumHeight; runY++)
                {
                    for (int runX = 0; runX < maximumWidth; runX++)
                    {
                        this.House[runX, runY] = " ";
                    }
                }
            }

            // Initialzing rest of the variables
            this.currentYCoordinate = -1;

            this.locationofWindows = new List<Tuple<int, int>>();
            this.possibleDoorLocations = new List<Tuple<int, int>>();
            this.finalDoorLocation = new Tuple<int, int>(-1, -1);
        }

        // This method creates the House in steps
        public void startConstruction()
        {
            // If only blueprint construction was sucessful
            if (this.blueprint.wasBluePrintConstrunctionSuccessful())
            {
                //The methods have to be executed in the order as shown below in order to constuct the House

                // Create the walls
                for (int run = 0; run < this.maximumHeight; run++)
                {
                    this.makeWalls(run);
                    
                }

                // Puts the edges with the walls created above
                this.putEdges();

                // Puts the floor using the edges above
                this.makeFloor();

                // Inserts the door at the bottom, after basic construction is done
                this.insertDoor();

                // Insert windows. Needs information from above methods
                this.insertWindows();

                // Puts the roof
                this.makeRoof();        
            }
            else
                Console.WriteLine("BluePrint specs invalid");
        }

        private void makeWalls(int currentLineNoInBluePrint)
        {           
           
            // Going from bottom to top 
            if (currentLineNoInBluePrint == 0)
            {
                this.currentYCoordinate = maximumHeight-2;       
            }

            // Variables used for checking whether a section of the house has been built or not 
            bool isLeftWallThere = false;
            bool isRightWallThere = false;

           // Coordinates in blueprint starts from 1 while in original it starts from 0
            int CurrentXCoordinateinBluePrint = 1;
     
            // Running from left to right
            for(int run =0; run < maximumWidth; run++)
            {

                // This is where data for windows and door will be collected
                if (this.currentYCoordinate == maximumHeight - 2 && (5 * run + 1) < maximumWidth)
                    possibleDoorLocations.Add(new Tuple<int, int>(5 * run + 1, this.currentYCoordinate));

                // Window locations being collected
                if (((5 * run) + 1) < maximumWidth && !isRightWallThere && isLeftWallThere)
                {
                    this.locationofWindows.Add(new Tuple<int, int>(5 * run + 1, this.currentYCoordinate));
                }
                
                // If the left wall is there but the right isn't and if there is an asterick above at the current position scaled back in the blueprint
                if (!isLeftWallThere && this.blueprint.getCoordinates().getValue(CurrentXCoordinateinBluePrint, this.blueprint.getCoordinates().getMaxYCoordinate() - 1 - currentLineNoInBluePrint) == 1)
                {                     
                    House[5*run, this.currentYCoordinate] = sideWall;
                    isLeftWallThere = true;
                    isRightWallThere = false;

                }

                /*
               // There was an extra char printing, so this fixes the problem
              if(run>0 && run%2!=0 && isLeftWallThere)
               {
                   if (House[5 * (run), this.currentYCoordinate] == this.sideWall)
                   {

                       House[5 * (run)-1, this.currentYCoordinate] = sideWall;
                       House[5 * (run), this.currentYCoordinate] = " ";

                   }
               }
               */
               

               

               // if Right is not there but left wall is there and there isn't an asterick above the current position in the current blueprint
                if (!isRightWallThere && isLeftWallThere && this.blueprint.getCoordinates().getValue(CurrentXCoordinateinBluePrint+1, this.blueprint.getCoordinates().getMaxYCoordinate() - 1 - currentLineNoInBluePrint) != 1)
                {   
                    House[5 * run+4, this.currentYCoordinate] = sideWall;
                    isLeftWallThere = false;
                    isRightWallThere = true;

                }

                /*
                // There was an extra char printing , this fixes the problem
                if (isRightWallThere && !isLeftWallThere && (5 * (run) + 4 < maximumWidth) && run % 2 != 0)
                {

                    if (House[5 * (run) + 4, this.currentYCoordinate] == this.sideWall)
                    {

                        House[5 * (run) + 3, this.currentYCoordinate] = sideWall;
                        House[5 * (run) + 4, this.currentYCoordinate] = " ";
                    }
                }
                */


                // Increase the position on the current x-axis in blueprint
                ++CurrentXCoordinateinBluePrint;

                
            }

           
            // Skips y-coordinate by 2 
            this.currentYCoordinate = this.currentYCoordinate - 2;

            
        }

        private void putEdges()
        {

            // Iterating over the House array
            for (int runY = 0; runY < maximumHeight; runY++)
            {
                for (int runX = 0; runX < maximumWidth; runX++)
                {
                    // If there is a side Wall at the current position
                    if (this.House[runX, runY] == this.sideWall)
                    {
                        // If there is another sideWall at the top by 2 of the current position of side Wall
                        if (this.House[runX, runY - 2] == this.sideWall)
                        {
                            // Insert a sideWall between the two side walls
                            this.House[runX, runY - 1] = this.sideWall;

                        }
                        else
                        {
                            //Put an edge at the bottom and the top of the current side Wall
                            this.House[runX, runY - 1] = this.Edge;
                            this.House[runX, runY + 1] = this.Edge;

                        }
                    }
                }

            }

            // In case there is a side wall above and space is empty at the bottom of a side Wall
            for (int runY = 0; runY < maximumHeight; runY++)
            {
                for (int runX = 0; runX < maximumWidth; runX++)
                {
                    if (this.House[runX, runY] == this.sideWall && this.House[runX, runY+1] == " ")
                    {
                        this.House[runX, runY+1] = this.Edge;

                    }   
                }
            }

        }


        public void makeFloor()
        {
            // To check whether the floor is being constructed or not
            bool floorMakingStarted = false;

            for (int runY = 0; runY < maximumHeight; runY++)
            {
                for (int runX = 0; runX < maximumWidth; runX++)
                {
                    //If only an edge is hit 
                    if (this.House[runX, runY] == this.Edge)
                    {
                        // if variable not already true
                        if (!floorMakingStarted)
                            floorMakingStarted = true;
                        else
                            floorMakingStarted = false;
                    }
                    
                    // Rechecks the condition above and floormaking has started
                    if(this.House[runX,runY]!=this.Edge && floorMakingStarted)
                    {
                        this.House[runX, runY] = this.Brick;

                    }

                }
            }
        }

        private void insertDoor()
        {
            // checks whether there a sideWall nearby or not
            bool checker;

            do
            {
                checker = true;
                
                //Generates a random from 0 to the count of the list-1 where the possible locations of the door is stored
                Random random = new Random();
                int chosenOne = random.Next(0, this.possibleDoorLocations.Count());

                // Stores the finalLocation chosen
                this.finalDoorLocation = this.possibleDoorLocations.ElementAt(chosenOne);

                // Checks whether there is a sideWall nearby to the right
                for (int run =0; run <3; run++)
                {
                    if (this.House[this.finalDoorLocation.Item1+run, this.finalDoorLocation.Item2] == this.sideWall)
                        checker = false;
                   
                }
               
                if (checker)
                {
                   //Inserts the door at the position chosen randomly
                    this.House[this.finalDoorLocation.Item1, this.finalDoorLocation.Item2] = this.sideWall + this.sideWall;

                    // Inserts an empty string since the door is two chars
                    this.House[this.finalDoorLocation.Item1 + 2, this.finalDoorLocation.Item2] = "";
                }

            } while (!checker);
        }

        private void insertWindows()
        {

            Random random = new Random();
            
            for(int run = 0; run < this.locationofWindows.Count; run++)
            {
                // Geneartes a random number either 0 or 1
                int decision = random.Next(0, 2);

                // Get the a window position 
                Tuple<int,int> currentLocation=this.locationofWindows.ElementAt(run);

                // If the current position chosen is empty and the decision is 0
                if(House[currentLocation.Item1,currentLocation.Item2]==" " && decision == 0)
                {
                    this.House[currentLocation.Item1, currentLocation.Item2] = this.window;
                    //this.House[currentLocation.Item1 + 1, currentLocation.Item2] = " ";

                }

            }

            // This block of code does the same thing as above but inserts windows n in the first blocks
               
            bool isFirstTimeEntering = false;

            int startingInt = 1;

            if ((this.maximumHeight % 2) == 0)
            {
                startingInt = 0;
            }

            for (int runY = startingInt; runY < maximumHeight; runY++)
            {
                for (int runX = 0; runX < maximumWidth; runX++)
                {
                    int decision = random.Next(0, 2);

                    if (this.House[runX, runY] == this.sideWall && isFirstTimeEntering)
                    {
                        isFirstTimeEntering = false;
                        
                    }

                    else
                    {

                        if (this.House[runX, runY] == this.sideWall && runX + 2 < maximumWidth)
                        {
                            if (decision == 0 && this.House[runX+1,runY]==" ")
                            {
                                this.House[runX + 3, runY] = "\0";
                                this.House[runX + 2, runY] = this.window;
                            }
                            isFirstTimeEntering = true;
                        }
                    }
                    
                
                }
                isFirstTimeEntering = false;
                ++runY;
            }
            
        }

        private void makeRoof()
        {
            // List will contain starting and ending coordinates of edges
            List<Tuple<Tuple<int, int>, Tuple<int, int>>> listOfRoofCoordinates = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();

            for (int runY = 0; runY < maximumHeight - 1; runY++)
            {
                // checks whether an edge has been encountered or not
                bool edgeEncountered = false;
                // Will store the starting coordinate
                Tuple<int, int> startingCoordinate=new Tuple<int,int>(-1,-1);

                for (int runX = 0; runX < maximumWidth; runX++)
                {
                    // if the edge hasn't been encountered before and there is an edge at the current position
                    if(!edgeEncountered && this.House[runX, runY] == this.Edge)
                    {
                        edgeEncountered = true;
                        startingCoordinate = new Tuple<int, int>(runX, runY);

                    }
                    else
                    {
                        // If there is an edge at the current position but the edge had been encountered is before
                        if(this.House[runX, runY] == this.Edge)
                        {
                            edgeEncountered = false;
                            // Adding the coordinates in the list
                            listOfRoofCoordinates.Add(new Tuple<Tuple<int, int>, Tuple<int, int>>(startingCoordinate,new Tuple<int,int>(runX,runY)));
                        }
                    }
                }
            }

            // Iterates over the list 
            for (int run = 0; run < listOfRoofCoordinates.Count; run++)
            {
                // starting and ending coordinate
                Tuple<int, int> start = listOfRoofCoordinates.ElementAt(run).Item1;
                Tuple<int, int> end = listOfRoofCoordinates.ElementAt(run).Item2;

                int condition = 0;

                // If the distance between start and even is even
                if ((end.Item1 - start.Item1) % 2 == 0)
                {
                    condition = (end.Item1 - start.Item1) / 2 - 1+1;
                }
                else
                {
                    condition = (end.Item1 - start.Item1) / 2;
                }

                // In case distance is one
                if (condition == 1)
                {
                    condition = 2;
                }

             
                if ((end.Item1 - start.Item1) % 2 == 0)
                {
                    // Puts the left side of the roof
                    for (int internalRun = 1; internalRun < condition; internalRun++)
                    {
                        this.House[start.Item1 + internalRun, (start.Item2 - internalRun)] = this.roofLeft;
                    }

                    // Puts the apex
                    this.House[start.Item1 + condition, start.Item2 - condition] = this.roofApex;

                    // Puts the right side of the roof
                    for (int internalRun = 1; internalRun < condition; internalRun++)
                    {
                        if (this.House[start.Item1 + condition + internalRun, (start.Item2 - condition + internalRun)] != this.sideWall)
                            this.House[start.Item1 + condition + internalRun, (start.Item2 - condition + internalRun)] = this.roofRight;
                    }
                }
                else
                {
                    // Does the same thing as above but from different positions

                    for (int internalRun = 1;internalRun < condition; internalRun++)
                    {
                        this.House[start.Item1 + internalRun, (start.Item2 - internalRun)] = this.roofLeft;
                    }


                    this.House[start.Item1 + condition, start.Item2 - condition] = this.roofApex+this.roofApex;
                    this.House[start.Item1 + condition + 2, start.Item2 - condition] = ""; 

                    for (int internalRun = 2; internalRun < condition+1; internalRun++)
                    {
                       
                         this.House[start.Item1 + condition + internalRun, (start.Item2 - condition + internalRun)-1] = this.roofRight;
                    }
                }

            }

        }

        
        // This method prints the House
        public void print()
        {
            // In case the output is big
            Console.SetWindowSize(150,30);

            for (int runY = 0; runY < maximumHeight; runY++)
            {
                for (int runX = 0; runX < maximumWidth; runX++)
                {
                    Console.Write(this.House[runX, runY]);
                    

                }
               
                Console.WriteLine();
            }
        }


    }
}

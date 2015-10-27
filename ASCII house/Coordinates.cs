using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCII_house
{
    // This class is used for storing a 2D Array. There can be only 2 values in the Array 1 or -1. 1 representing
    // an asterick and -1 representing a blank 
    class Coordinates
    {
        private int[,] array;
        private int maxXCoordinate;
        private int maxYCoordinate;

        public Coordinates(int x,int y)
        {
            if(x>0 && y>0)
            {
                array = new int[x, y];
                this.maxXCoordinate = x;
                this.maxYCoordinate = y;
            }
            else
            {
                Console.WriteLine("Invalid input");

            }

        }

        public bool addCoordinates(int x,int y,int value)
        {
            
            if (x > 0 && y > 0 && (value == -1 || value == 1) && x<maxXCoordinate && y<maxYCoordinate)
            {
                this.array[x, y] = value;
                return true;
            }
            else
                return false;

        }

        public int getValue(int x,int y)
        {
            if (x > 0 && y > 0 && x < maxXCoordinate && y < maxYCoordinate)
                return array[x, y];
            else
                return 0;
        }

        public int getMaxXCoordinate()
        {
            return this.maxXCoordinate;
        }

        public int getMaxYCoordinate()
        {
            return this.maxYCoordinate;
        }

        public int[,] getAllCoordinates()
        {
            return this.array;
        }
    }
}

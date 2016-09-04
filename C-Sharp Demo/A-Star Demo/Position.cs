/* 
 * Author:  Samuel Arzt
 * Date:    2016.06.09
 */

using System;

namespace A_Star_Demo
{
    class Position : IComparable<Position>
    {

        public int X
        {
            get;
            private set;
        }

        public int Y
        {
            get;
            private set;
        }

        public Position (int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int CompareTo(Position other)
        {
            //Only returns 0 if both coordinates are the same
            if (this.X == other.X && this.Y == other.Y)
                return 0;

            return -2;
        }


        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }
}

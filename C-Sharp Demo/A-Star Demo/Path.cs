/* 
 * Author:  Samuel Arzt
 * Date:    2016.06.09
 */

using System;

namespace A_Star_Demo
{
    /// <summary>
    /// Class for storing a path's coordinates and printing these in position or word form.
    /// </summary>
    class Path
    {

        private Position[] pathBank;

        public Position this[int idx]
        {
            get { return pathBank[idx]; }
        }

        public Path(Position[] pathBank)
        {
            this.pathBank = pathBank;
        }

        /// <summary>
        /// Prints this path by printing each position.
        /// </summary>
        public void PrintAsPositions()
        {
            foreach (Position p in pathBank)
                Console.Write(p + "; ");
            Console.WriteLine();
        }

        /// <summary>
        /// Prints this path by printing the directions you would have to take in order to get from start to target position.
        /// </summary>
        public void PrintAsDirections()
        {
            for (int i = 1; i < pathBank.Length; i++)
                Console.Write(GetDirection(pathBank[i - 1], pathBank[i]) + ", ");
            Console.WriteLine();
        }

        private enum Direction
        {
            Up, Down, Left, Right, Up_Left, Up_Right, Down_Left, Down_Right
        }

        //Returns the direction from position a to position b
        private Direction GetDirection(Position a, Position b)
        {
            if (a.X < b.X) //Right
            {
                if (a.Y < b.Y)
                    return Direction.Down_Right;
                if (a.Y > b.Y)
                    return Direction.Up_Right;

                return Direction.Right;
            }
            if (a.X > b.X) //Left
            {
                if (a.Y < b.Y)
                    return Direction.Down_Left;
                if (a.Y > b.Y)
                    return Direction.Up_Left;

                return Direction.Left;
            }
            if (a.Y < b.Y) //Down
                return Direction.Down;

            if (a.Y > b.Y) //Up
                return Direction.Up;

            //Both points must be the same at this point... wrong input
            throw new ArgumentException("Can't return direction of same positions.");
        }

    }
}

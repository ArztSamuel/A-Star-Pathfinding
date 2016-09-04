/* 
 * Author:  Samuel Arzt
 * Date:    2016.06.09
 */

using System;

namespace A_Star_Demo
{
    class AStar_Main
    {
        static void Main()
        {
            byte[,] fieldCodes = new byte[7, 10]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };


            Field field = new Field(fieldCodes);
            field.Transpose(); //Transpose field for right indexation ([XCoord, YCoord])
            Console.WriteLine("Field: ");
            field.Print();

            AStar aStar = new AStar(field);
            Path path = aStar.FindPath(new Position(2, 2), new Position(9, 6));

            Console.WriteLine("As Positions:");
            path.PrintAsPositions();

            Console.WriteLine("As Directions:");
            path.PrintAsDirections();

        }

    }
}

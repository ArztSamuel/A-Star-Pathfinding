/* 
 * Author:  Samuel Arzt
 * Date:    2016.06.09
 */

using System;

namespace A_Star_Demo
{
    /// <summary>
    /// Class representing a square field of tiles. Each tile has a byte code identifying it.
    /// </summary>
    class Field
    {
        public enum FieldCodec : byte
        {
            Path = 0, Wall = 1
        }

        public int SizeX
        {
            get { return fields.GetLength(0); }
        }

        public int SizeY
        {
            get { return fields.GetLength(1); }
        }

        private byte[,] fields;

        /// <summary>
        /// A tiled field consisting of byte codes. A tile's X/Y position is equal to its position
        /// in the byte array [X,Y].
        /// </summary>
        /// <param name="fields"></param>
        public Field(byte[,] fields)
        {
            this.fields = fields;
        }


        /// <summary>
        /// Transposes the internal field array.
        /// </summary>
        public void Transpose()
        {
            byte[,] newField = new byte[fields.GetLength(1), fields.GetLength(0)];

            for (int x = 0; x < fields.GetLength(0); x++)
            {
                for (int y = 0; y < fields.GetLength(1); y++)
                {
                    newField[y, x] = fields[x, y];
                }
            }

            fields = newField;
        }

        /// <summary>
        /// Checks whether the given position is in field bounds.
        /// </summary>
        /// <param name="p">The position to check for.</param>
        /// <returns>True if the given position is in field bounds, otherwhise false.</returns>
        public bool IsInBounds(Position p)
        {
            if (p.X < 0 || p.Y < 0 || p.X >= fields.GetLength(0) || p.Y >= fields.GetLength(1))
                return false;

            return true;
        }

        /// <summary>
        /// Checks whether the tile at given position is walkable.
        /// </summary>
        /// <param name="p">The position of the tile to return the walkability of.</param>
        /// <returns>True if the tile at the given position is walkable, otherwhise false.</returns>
        public bool IsTileWalkable(Position p)
        {
            return !(fields[p.X, p.Y] == (byte)FieldCodec.Wall);
        }


        /// <summary>
        /// Prints this field to screen.
        /// </summary>
        public void Print()
        {
            for (int y = 0; y < this.fields.GetLength(1); y++)
            {
                Console.Write("|");
                for (int x = 0; x < this.fields.GetLength(0); x++)
                    Console.Write(fields[x, y] + "|");

                Console.WriteLine();
                for (int i = 0; i < this.fields.GetLength(0); i++)
                    Console.Write(" -");
                Console.WriteLine();
            }
        }


    }
}

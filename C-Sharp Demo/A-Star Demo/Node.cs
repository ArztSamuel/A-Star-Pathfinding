/* 
 * Author:  Samuel Arzt
 * Date:    2016.06.09
 */

using System;
using System.Collections.Generic;

namespace A_Star_Demo
{
    /// <summary>
    /// Class representing a single field with an assigned ID for lookup.
    /// </summary>
    class Node : IComparable<Node>
    {
        public static readonly PositionComparer PosComparer = new PositionComparer();

        public ushort FCost
        {
            get { return (ushort)(GCost + HCost); }
        }

        public ushort GCost
        {
            get;
            set;
        }

        public ushort HCost
        {
            get;
            set;
        }

        public Node Parent
        {
            get;
            set;
        }

        public Position Position
        {
            get;
            private set;
        }


        public Node(Position pos, ushort gCost, ushort hCost)
        {
            this.Position = pos;
            this.GCost = gCost;
            this.HCost = hCost;
        }

        public int CompareTo(Node other)
        {
            return this.FCost.CompareTo(other.FCost);
        }


        public class PositionComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                return x.Position.CompareTo(y.Position);
            }
        }

        public override string ToString()
        {
            return "(" + Position + " G: " + GCost + " H: " + HCost + " F: " + FCost + ")";
        }
    }
}

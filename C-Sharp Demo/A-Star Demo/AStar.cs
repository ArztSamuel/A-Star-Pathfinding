/* 
 * Author:  Samuel Arzt
 * Date:    2016.06.09
 */

using System;

namespace A_Star_Demo
{

    /// <summary>
    /// Class utilizing the A* algorithm for finding a fastest path from start to target in a tile map.
    /// </summary>
    class AStar
    {
        private const ushort DIAGONAL_COST = 14, STRAIGHT_COST = 10; //The costs of walking diagonally or straight
        //Diagonal cost = Straight cost * sqrt(2)

        private readonly Field field; //The field to search the path in.

        private ushort[,] gCost; //Saves the gCosts of tile with position (X/Y) at [X][Y]
        private int[,] whichList; //Saves in which list the tile at (X/Y) currently is

        //Codes for whichList
        public int onOpenList = 0;
        public int onClosedList = 0;


        public AStar(Field field)
        {
            this.field = field;

            //Initialization of array sizes depending on field size
            gCost = new ushort[field.SizeX, field.SizeY];
            whichList = new int[field.SizeX, field.SizeY];
        }



        /// <summary>
        /// Finds a fastest path from startPosition to targetPosition in 
        /// the field that this instance was instantiated with.
        /// </summary>
        /// <param name="startPosition">The starting position of the path.</param>
        /// <param name="targetPosition">The target position of the path.</param>
        /// <returns>A fastest path from start- to targetPosition if such a path exists, otherwhise null.</returns>
        public Path FindPath(Position startPosition, Position targetPosition)
        {

            //Initialization
            BinaryHeap<Node> openList = new BinaryHeap<Node>();

            //Check if start and end positions are in field bounds
            if (!field.IsInBounds(startPosition))
                throw new ArgumentOutOfRangeException("Start position is out of field bounds!");
            if (!field.IsInBounds(targetPosition))
                throw new ArgumentOutOfRangeException("Target position is out of field bounds!");
           
            //Check if path even has to be calculated
            if (startPosition.X == targetPosition.X && startPosition.Y == targetPosition.Y)
            //Start = Target
                return new Path(new Position[0]);
            if (!field.IsTileWalkable(targetPosition))
            //Target not walkable
                return null;

            //Reset whichList occasionally
            if (onClosedList > 1000000)
            {
                for (int x = 0; x < whichList.GetLength(0); x++)
                {
                    for (int y = 0; y < whichList.GetLength(1); y++)
                        whichList[x, y] = 0;
                }
                onClosedList = 0;
            }
            onClosedList = onClosedList + 2; //changing the values of onOpenList and onClosedList is faster than redimming whichList array
            onOpenList = onClosedList - 1;


            //Add start node to openList
            Node curParent = null;
            Node startField = new Node(startPosition, 0, 0);
            gCost[startPosition.X, startPosition.Y] = 0;
            openList.Add(startField);

            //Pathfinding loop
            while (whichList[targetPosition.X, targetPosition.Y] != onOpenList) //If target is on openList we have found the path
            {
                if (openList.Count != 0)
                { //If there are no more nodes in openList, then there must be all reachable nodes in closed list. 
                  //(if target is not in closedList now, then the target must be unreachable)

                    //Set first node of openList (the one with lowest FCost) on closed list
                    //The parent for the next node is now the node we just added to closed list
                    openList.Print();
                    Node smallestF = openList.Remove();
                    curParent = smallestF;

                    whichList[smallestF.Position.X, smallestF.Position.Y] = onClosedList;


                    //Check all 8 neighbors
                    for (int nbY = curParent.Position.Y - 1; nbY <= curParent.Position.Y + 1; nbY++) //For loop, checking all neighbors of current parent
                    {
                        for (int nbX = curParent.Position.X - 1; nbX <= curParent.Position.X + 1; nbX++)
                        {
                            Position neighbor = new Position(nbX, nbY);
                            if (field.IsInBounds(neighbor) &&               //If neighbor in field bounds
                                whichList[nbX, nbY] != onClosedList &&      //and not already on closedList
                                field.IsTileWalkable(neighbor))             //and neighbor is walkable 
                            {
                                if (CheckCorner(nbX, nbY, curParent)) //If neighbor passed all previous conditions and is corner walkable, it is now added to openList
                                {
                                    if (whichList[nbX, nbY] != onOpenList) //If neighbor is not already on openList, we have to add it
                                    {
                                        //Calculate gCost (Path from Parent to new node Node)
                                        ushort addedGCost;
                                        if (Math.Abs(nbX - curParent.Position.X) == 1 && Math.Abs(nbY - curParent.Position.Y) == 1) //If diagonal
                                            addedGCost = DIAGONAL_COST;
                                        else
                                            addedGCost = STRAIGHT_COST;
                                        ushort gCost = (ushort) (curParent.GCost + addedGCost); //GCost of new node = new cost + gCost of parent

                                        //Calculate hCost with manhattan method 
                                        ushort hCost = (ushort) (STRAIGHT_COST * (Math.Abs(nbX - targetPosition.X) + Math.Abs(nbY - targetPosition.Y)));
                                        
                                        //Add neighbor to openList
                                        Node neighborNode = new Node(neighbor, gCost, hCost);
                                        this.gCost[nbX, nbY] = gCost;
                                        openList.Add(neighborNode);
                                        whichList[nbX, nbY] = onOpenList;

                                        //Save the parent of the new node
                                        neighborNode.Parent = curParent;
                                    }
                                    else //If the node is already in openList we only update its costs
                                    {
                                        //Calculate possible new gCosts for comparison
                                        ushort addedGCost;
                                        if (Math.Abs(nbX - curParent.Position.X) == 1 && Math.Abs(nbY - curParent.Position.Y) == 1) //Diagonal
                                            addedGCost = DIAGONAL_COST;
                                        else
                                            addedGCost = STRAIGHT_COST;

                                        ushort newGCost = (ushort)(curParent.GCost + addedGCost);


                                        //If new gCosts are less than before, the parent will be switched
                                        if (newGCost < gCost[nbX, nbY])
                                        {
                                            //Search for right node in openList to update costs and parent
                                            int oldIdx = openList.Find(new Node(neighbor, 0, 0), Node.PosComparer);
                                            Node newNode = new Node(openList[oldIdx].Position, newGCost, openList[oldIdx].HCost);
                                            newNode.Parent = curParent;
											openList.Replace(newNode, oldIdx);
                                            gCost[nbX, nbY] = newGCost;
                                        }
                                    }
                                }
                            }
                        }
                    }// End of neighbor calculation //
                }
                else //No more items on openList, target unreachable
                {
                    return null;
                }
            }//End of Pathfinding loop

            //Now save and return path

            //First get path length by working our way backwards
            Node curNode = curParent; //must be parent of target at this point
            int pathLength = 2; //Start and Target Positions are always present (special case start = target was already handled at beginning)
            while (curNode.Position.X != startPosition.X || curNode.Position.Y != startPosition.Y)
            {
                curNode = curNode.Parent;
                pathLength++;
            }

            //Now copy the data to the pathBank in the right order, starting at target and working
            //our way backwards following each node's parent.
            Position[] pathBank = new Position[pathLength];
            //Add start and target positions
            pathBank[0] = startPosition;
            pathBank[pathLength - 1] = targetPosition;

            int curPathIdx = pathLength - 2;
            curNode = curParent; //set back to parent of target
            while (curNode.Position.X != startPosition.X || curNode.Position.Y != startPosition.Y)
            {
                pathBank[curPathIdx] = curNode.Position;
                curNode = curNode.Parent;
                curPathIdx--;
            }
            return new Path(pathBank);
        }
        //End of pathfinding method



        // Method for checking whether diagonal is walkable
        private bool CheckCorner(int nbX, int nbY, Node curParent)
        {
            if (nbX == curParent.Position.X - 1)
            {
                if (nbY == curParent.Position.Y - 1)
                {
                    if (!field.IsTileWalkable(new Position(curParent.Position.X - 1, curParent.Position.Y)) || 
                        !field.IsTileWalkable(new Position(curParent.Position.X, curParent.Position.Y - 1)))
                        return false;
                }
                else if (nbY == curParent.Position.Y + 1)
                {
                    if (!field.IsTileWalkable(new Position(curParent.Position.X, curParent.Position.Y + 1)) || 
                        !field.IsTileWalkable(new Position(curParent.Position.X - 1, curParent.Position.Y)))
                        return false;
                }

            }
            else if (nbX == curParent.Position.X + 1)
            {
                if (nbY == curParent.Position.Y - 1)
                {
                    if (!field.IsTileWalkable(new Position(curParent.Position.X, curParent.Position.Y - 1)) || 
                        !field.IsTileWalkable(new Position(curParent.Position.X + 1, curParent.Position.Y)))
                        return false;
                }
                else if (nbY == curParent.Position.Y + 1)
                {
                    if (!field.IsTileWalkable(new Position(curParent.Position.X + 1, curParent.Position.Y)) || 
                        !field.IsTileWalkable(new Position(curParent.Position.X, curParent.Position.Y + 1)))
                        return false;
                }
            }
            return true;
        }

    }

}

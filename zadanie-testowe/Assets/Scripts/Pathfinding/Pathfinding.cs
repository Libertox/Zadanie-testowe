using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestTask.PathFinding
{
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private readonly List<List<PathNode>> pathNodeGrid;

        private readonly float height;
        private readonly float width;

        private List<PathNode> openList;
        private List<PathNode> closedList;

        public Pathfinding(List<List<PathNode>> pathNodeGrid, float height, float width)
        {
            this.pathNodeGrid = pathNodeGrid;
            this.height = height;
            this.width = width;
        }

        public Stack<PathNode> FindPath(float startPosX, float startPosZ, float endPosX, float endPosZ)
        {
            PathNode startPathNode = GetPathNodeFromPosition(startPosX, startPosZ);
            PathNode endPathNode = GetPathNodeFromPosition(endPosX, endPosZ);


            closedList = new List<PathNode>();
            openList = new List<PathNode> { startPathNode };

            for (int i = 0; i < pathNodeGrid.Count; i++)
            {
                for (int j = 0; j < pathNodeGrid[i].Count; j++)
                {
                    pathNodeGrid[i][j].GCost = int.MaxValue;
                    pathNodeGrid[i][j].CalculateFCost();
                    pathNodeGrid[i][j].CameFromNode = null;
                }
            }

            startPathNode.GCost = 0;
            startPathNode.HCost = CalculateDistance(startPathNode, endPathNode);
            startPathNode.CalculateFCost();

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                if (currentNode == endPathNode)
                {
                    return CalculatePath(endPathNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;

                   
                    int tentativeGCost = currentNode.GCost + CalculateDistance(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.GCost)
                    {
                        neighbourNode.CameFromNode = currentNode;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.HCost = CalculateDistance(neighbourNode, endPathNode);
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }

                }

            }
            return null;

        }

        private int CalculateDistance(PathNode a, PathNode b)
        {
            float xDistance = Math.Abs(a.Position.x - b.Position.x);
            float zDistance = Math.Abs(a.Position.z - b.Position.z);
            float remaining = Math.Abs(zDistance - xDistance);
            return (int)(MOVE_DIAGONAL_COST * Math.Min(zDistance, xDistance) + MOVE_STRAIGHT_COST * remaining);
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            return pathNodeList.OrderBy(x => x.FCost).First();
        }

        private Stack<PathNode> CalculatePath(PathNode endNode)
        {
            Stack<PathNode> pathNodes = new Stack<PathNode>();
            pathNodes.Push(endNode);
            PathNode currentNode = endNode;
            while (currentNode.CameFromNode != null)
            {
                pathNodes.Push(currentNode.CameFromNode);
                currentNode = currentNode.CameFromNode;
            }
            pathNodes.Pop();

            return pathNodes;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            if (currentNode.GridIndexWidth - 1 >= 0)
            {
                neighbourList.Add(GetPathNodeFromGridIndex(currentNode.GridIndexWidth - 1, currentNode.GridIndexHeight));

                if (currentNode.GridIndexHeight - 1 >= 0)
                    neighbourList.Add(GetPathNodeFromGridIndex(currentNode.GridIndexWidth - 1, currentNode.GridIndexHeight - 1));

                if (currentNode.GridIndexHeight + 1 <= height)
                    neighbourList.Add(GetPathNodeFromGridIndex(currentNode.GridIndexWidth - 1, currentNode.GridIndexHeight + 1));
            }

            if (currentNode.GridIndexWidth + 1 <= width)
            {
                neighbourList.Add(GetPathNodeFromGridIndex(currentNode.GridIndexWidth + 1, currentNode.GridIndexHeight));

                if (currentNode.GridIndexHeight - 1 >= 0)
                    neighbourList.Add(GetPathNodeFromGridIndex(currentNode.GridIndexWidth + 1, currentNode.GridIndexHeight - 1));

                if (currentNode.GridIndexHeight + 1 <= height)
                    neighbourList.Add(GetPathNodeFromGridIndex(currentNode.GridIndexWidth + 1, currentNode.GridIndexHeight + 1));
            }

            if (currentNode.GridIndexHeight - 1 >= 0)
                neighbourList.Add(GetPathNodeFromGridIndex(currentNode.GridIndexWidth, currentNode.GridIndexHeight - 1));

            if (currentNode.GridIndexHeight + 1 <= height)
                neighbourList.Add(GetPathNodeFromGridIndex(currentNode.GridIndexWidth, currentNode.GridIndexHeight + 1));


            return neighbourList;

        }

        private PathNode GetPathNodeFromGridIndex(int heightIndex, int widthIndex) => pathNodeGrid[heightIndex][widthIndex];

        private PathNode GetPathNodeFromPosition(float posX, float posZ)
        {
            for (int i = 0; i < pathNodeGrid.Count; i++)
            {
                for (int j = 0; j < pathNodeGrid[i].Count; j++)
                {
                    if (pathNodeGrid[i][j].IsPositionWithin(posX, posZ))
                        return pathNodeGrid[i][j];

                }
            }
            return null;
        }

    }
}

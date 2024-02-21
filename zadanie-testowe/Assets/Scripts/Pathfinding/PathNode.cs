using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace TestTask.Pathfinding
{
    public class PathNode
    {
        public Vector3 Position { get; set; }

        public int GridIndexHeight { get; set; }
        public int GridIndexWidth { get; set; }

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get; set; }

        public PathNode CameFromNode { get; set; }

        private readonly float halfSize;

        public PathNode(Vector3 position, float size, int gridIndexHeight, int gridIndexWidth)
        {
            this.Position = position;
            this.halfSize = size * 0.5f;
            this.GridIndexHeight = gridIndexHeight;
            this.GridIndexWidth = gridIndexWidth;
        }


        public bool IsPositionWithin(float posX, float posZ)
        {
            return (Math.Abs(Position.x - posX) <= halfSize && Math.Abs(Position.z - posZ) <= halfSize);
        }

        public void CalculateFCost()
        {
            FCost = GCost + HCost;
        }
    }
}

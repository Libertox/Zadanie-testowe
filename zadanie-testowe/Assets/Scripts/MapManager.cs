
using System.Collections;
using System.Collections.Generic;
using TestTask.PathFinding;
using UnityEngine;

namespace TestTask
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        [SerializeField] private float mapWidth;
        [SerializeField] private float mapHeight;

        [SerializeField] private Transform map;

        [SerializeField] private float cellSize;

        private List<List<PathNode>> pathNodeGrid;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);
        }
        private void Start()
        {
            SetupMapDimension(); 
            pathNodeGrid = GeneratePathNodeGrid();      
        }

        private void SetupMapDimension()
        {
            map.localScale = new Vector3(mapHeight, map.localScale.y, mapWidth);        
        }

        private List<List<PathNode>> GeneratePathNodeGrid()
        {
            float rightBottonCornerPosX = map.transform.position.x - mapHeight * 0.5f;
            float rightBottonCornerPosZ = map.transform.position.z + mapWidth * 0.5f;

            List<List<PathNode>> pathNodeGrid = new List<List<PathNode>>();

            for (int i = 0; i <= mapHeight / cellSize; i++)
            {
                float startPositonZ = rightBottonCornerPosZ;
                List<PathNode> nodes = new List<PathNode>();
                for (int j = 0; j <= mapWidth / cellSize; j++)
                {
                    PathNode node = new PathNode(new Vector3(rightBottonCornerPosX, 0, startPositonZ), cellSize, j, i);
         
                    nodes.Add(node);

                    startPositonZ -= cellSize;
                }
                rightBottonCornerPosX += cellSize;
                pathNodeGrid.Add(nodes);
            }

            return pathNodeGrid;
        }

        public Vector3 GetRandomPositionWithinMap()
        {
            float maxPositionX = map.transform.position.x + mapHeight * 0.5f;
            float minPositionX = map.transform.position.x - mapHeight * 0.5f;

            float posX = UnityEngine.Random.Range(minPositionX, maxPositionX);

            float maxPositionZ = map.transform.position.z + mapWidth * 0.5f;
            float minPositionZ = map.transform.position.z - mapWidth * 0.5f;

            float posZ = UnityEngine.Random.Range(minPositionZ, maxPositionZ);

            return new Vector3(posX, 0, posZ);
        }

        public Stack<PathNode> GetShortPath(Vector3 startPoint, Vector3 endPoint)
        {
            Pathfinding pathfinding = new Pathfinding(pathNodeGrid, mapHeight / cellSize, mapWidth / cellSize);
            return pathfinding.FindPath(startPoint.x, startPoint.z, endPoint.x, endPoint.z);


        }

    }
}

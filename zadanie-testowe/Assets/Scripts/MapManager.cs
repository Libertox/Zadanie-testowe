
using System.Collections;
using System.Collections.Generic;
using TestTask.PathFinding;
using UnityEngine;
using UnityEngine.UIElements;

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

        private Collider[] colliders = new Collider[10];

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
                    PathNode node = new PathNode(new Vector3(rightBottonCornerPosX, map.position.y, startPositonZ), cellSize, j, i);
         
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
            Vector3 randomPosition = new Vector3(posX, map.position.y, posZ);

            int checkNumber = 0;
            int checkNumberMax = 1000;

            while (!CheckPositionIsFree(randomPosition))
            {
                posX = UnityEngine.Random.Range(minPositionX, maxPositionX);
                posZ = UnityEngine.Random.Range(minPositionZ, maxPositionZ);

                randomPosition = new Vector3(posX, map.position.y, posZ);

                if (checkNumber > checkNumberMax) 
                {
                    return randomPosition;
                }

                checkNumber++;
            }

            return randomPosition;
        }

        private bool CheckPositionIsFree(Vector3 position)
        {
            Vector3 halfExtents = new Vector3(0.5f, 0.5f, 0.5f);
            Physics.OverlapBoxNonAlloc(position, halfExtents, colliders);
           
            foreach (var collider in colliders)
            {
                if(collider == null) continue;

                if (collider.GetComponent<Agent>()) return false;
            }
            return true;
        }

        public Stack<PathNode> GetShortPath(Vector3 startPoint, Vector3 endPoint)
        {
            Pathfinding pathfinding = new Pathfinding(pathNodeGrid, mapHeight / cellSize, mapWidth / cellSize);
            return pathfinding.FindPath(startPoint.x, startPoint.z, endPoint.x, endPoint.z);
        }

       

    }
}

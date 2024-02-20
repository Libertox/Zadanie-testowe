using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestTask
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        [SerializeField] private float mapWidth;
        [SerializeField] private float mapHeight;

        [SerializeField] private Transform map;


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
        }

        private void SetupMapDimension()
        {
            map.localScale = new Vector3(mapHeight, map.localScale.y, mapWidth);        
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

    }
}

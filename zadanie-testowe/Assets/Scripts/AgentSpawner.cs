using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestTask
{
    public class AgentSpawner:MonoBehaviour
    {

        [Header("Agent Spawn Properties")]
        [SerializeField] private int minStartingAgents;
        [SerializeField] private int maxStartingAgents;

        [SerializeField] private int maxAgent;

        [SerializeField] private GameObject agentPrefab;

        [Header("Time Spawn Properties")]
        [SerializeField] private float minSpawnDuration;
        [SerializeField] private float maxSpawnDuration;

        
        private int currentAgentNumber;
        private float spawnDuration;
        private float time;

        private void Start()
        {
            CreateStartAgents();
            SetRandomSpawnDuration();
        }

        private void CreateStartAgents()
        {
            int startAgentsNumber = UnityEngine.Random.Range(minStartingAgents, maxStartingAgents + 1);
           
            for (int i = 1; i < startAgentsNumber; i++)
            {
                SpawnAgent();
            }
        }

        private void SpawnAgent()
        {
            Instantiate(agentPrefab, MapManager.Instance.GetRandomPositionWithinMap(), Quaternion.identity);
            currentAgentNumber++;
        }

        private void SetRandomSpawnDuration()
        {
            spawnDuration = UnityEngine.Random.Range(minSpawnDuration, maxSpawnDuration);
        }

        private void Update()
        {
            if (!CanSpawnNewAgent()) return;

            time += Time.deltaTime;
            if(time > spawnDuration)
            {
                SpawnAgent();
                time -= spawnDuration;
                SetRandomSpawnDuration();
            }
        }

        private bool CanSpawnNewAgent()
        {
            return currentAgentNumber <= maxAgent;
        }



    }
}

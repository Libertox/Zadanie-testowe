using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TestTask
{
    public class AgentSpawner:MonoBehaviour
    {

        [Header("Agent Spawn Properties")]
        [SerializeField] private int minStartingAgents;
        [SerializeField] private int maxStartingAgents;

        [SerializeField] private int maxAgent;

        [SerializeField] private Agent agentPrefab;

        [Header("Time Spawn Properties")]
        [SerializeField] private float minSpawnDuration;
        [SerializeField] private float maxSpawnDuration;

        
        private int currentAgentNumber;
        private float spawnDuration;
        private float time;

        private ObjectPool<Agent> agentPool;

        private void Awake()
        {
            Agent.OnDestroyed += Agent_OnDestroyed;       
        }

        private void Start()
        {
            agentPool = new ObjectPool<Agent>(
               () => Instantiate(agentPrefab, MapManager.Instance.GetRandomPositionWithinMap(), Quaternion.identity),
               (Agent agent) => agent.Initialize(MapManager.Instance.GetRandomPositionWithinMap()),
               (Agent agent) => agent.gameObject.SetActive(false));

            CreateStartAgents();
            SetRandomSpawnDuration();
        }

        private void Agent_OnDestroyed(object sender, EventArgs e)
        {
            Agent agent = sender as Agent;
            agentPool.Release(agent);
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
            agentPool.Get();
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

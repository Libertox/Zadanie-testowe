using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestTask
{
    [CreateAssetMenu(fileName = "AgentStats", menuName = "ScriptableObject/AgentStats")]
    public class AgentStatsSO: ScriptableObject
    {
        [field: SerializeField] public int MaxHP { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        [SerializeField] private List<string> names;

        [SerializeField] private List<Sprite> potraitBackgrounds;
        [SerializeField] private List<Sprite> potraits;

        public string GetRandomAgentName()
        {
            int randomIndex = UnityEngine.Random.Range(0, names.Count);
            return names[randomIndex];
        }

        public Sprite GetRandomPotraitBackground()
        {
            int randomIndex = UnityEngine.Random.Range(0,potraitBackgrounds.Count);
            return potraitBackgrounds[randomIndex];
        }

        public Sprite GetRandomPotrait()
        {
            int randomIndex = UnityEngine.Random.Range(0, potraits.Count);
            return potraits[randomIndex];
        }

    }
}

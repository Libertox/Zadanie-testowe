﻿using System;
using System.Collections.Generic;
using UnityEngine;
using TestTask.PathFinding;

namespace TestTask
{
    public class Agent : MonoBehaviour, ISelectable, IDamageable
    {   
        public static event EventHandler<OnSelectedEventArgs> OnSelected;
        public static event EventHandler OnDeselected;
        public static event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
        public static event EventHandler OnDestroyed;

        public class OnSelectedEventArgs: EventArgs
        {
            public string name;
            public int hp;
            public Sprite potraitBackground;
            public Sprite potrait;
        }

        public class OnHealthChangedEventArgs: EventArgs
        {
            public int hp;
        }

        public int damage { get; set; } = 1;

        [SerializeField] private GameObject selectedIndicator;
        [SerializeField] private AgentStatsSO agentStatsSO;

        private string agentName;
        private int hp;
        private bool isSelected;
        private Sprite potraitBackground;
        private Sprite potrait;


        private Stack<PathNode> pathNodes;

        private readonly Collider[] detectColiders = new Collider[10];
        private List<IDamageable> lastCollidingObject = new List<IDamageable>();

        public void Initialize(Vector3 startPosition)
        {
            gameObject.SetActive(true);
            hp = agentStatsSO.MaxHP;
            agentName = agentStatsSO.GetRandomAgentName();
            potrait = agentStatsSO.GetRandomPotrait();
            potraitBackground = agentStatsSO.GetRandomPotraitBackground();
            pathNodes = new Stack<PathNode>();
            transform.SetPositionAndRotation(startPosition, Quaternion.identity);

            DetectCollisions();
            lastCollidingObject = new List<IDamageable>();

        }

        public void SetPath(Stack<PathNode> pathNodes) => this.pathNodes = pathNodes;
    
        public void Select()
        {
            selectedIndicator.SetActive(true);
            isSelected = true;
            OnSelected?.Invoke(this, new OnSelectedEventArgs
            {
                name = agentName,
                hp = hp,
                potraitBackground = potraitBackground,
                potrait = potrait,

            });
        }

        public void Deselect()
        {
            selectedIndicator.SetActive(false);
            isSelected = false;
            OnDeselected?.Invoke(this, EventArgs.Empty);
        }

        public void TakeDamage(int damage)
        {
            if (hp <= 0) return;

            hp -= damage;

            if (isSelected)
                OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs { hp = hp });

            if (hp <= 0)
                DestroySelf();
        }

        public void DestroySelf()
        {
            if (isSelected) Deselect();

            OnDestroyed?.Invoke(this, EventArgs.Empty);
        }

        private void Update()
        {           
            if (pathNodes.Count == 0) return;
      
            MoveTowardsDestination(pathNodes.Peek().Position);
       
            if (transform.position == pathNodes.Peek().Position)
            {
                pathNodes.Pop();
                if (pathNodes.Count <= 0) return;
            }

            Vector3 direction = pathNodes.Peek().Position - transform.position;
            RotateTowardsDirection(direction);
        }

        private void MoveTowardsDestination(Vector3 destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * agentStatsSO.Speed);
        }

        private void RotateTowardsDirection(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        private void FixedUpdate()
        {
            if (!isSelected) return;

            DetectCollisions();
        }

        private void DetectCollisions()
        {
            Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
            Physics.OverlapBoxNonAlloc(transform.position, halfExtents, detectColiders, transform.rotation);
            List<IDamageable> detectObject = new List<IDamageable>();

            foreach(var colider in detectColiders)
            {
                if (colider == null) continue;

                if(colider.TryGetComponent(out IDamageable damageable))
                {
                    if (damageable == this as IDamageable) continue;
                    
                    detectObject.Add(damageable);
                    if (!lastCollidingObject.Contains(damageable))
                    {
                        damageable.TakeDamage(damage);
                        TakeDamage(damageable.damage);                      
                    }
    
                }
            }
            lastCollidingObject = detectObject;
        }
        
        
    }
}

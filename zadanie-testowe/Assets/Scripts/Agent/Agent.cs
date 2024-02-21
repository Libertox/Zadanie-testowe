using System;
using System.Collections.Generic;
using UnityEngine;
using TestTask.PathFinding;

namespace TestTask
{
    public class Agent : MonoBehaviour, ISelectable, IDamageable
    {
        private const int StartHP = 3;

        public static event EventHandler<OnSelectedEventArgs> OnSelected;
        public static event EventHandler OnDeselected;
        public static event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
        public static event EventHandler OnDestroyed;

        public class OnSelectedEventArgs: EventArgs
        {
            public string name;
            public int hp;
        }

        public class OnHealthChangedEventArgs: EventArgs
        {
            public int hp;
        }

        public int damage { get; set; } = 1;

        [SerializeField] private float speed;
        [SerializeField] private string agentName;

        [SerializeField] private GameObject selectedIndicator;

        private int hp;
        private bool isSelected;
    
        private Stack<PathNode> pathNodes;

        private Collider[] detectColiders = new Collider[1];
        private List<IDamageable> lastCollidingObject;

        public void Initialize(Vector3 startPosition)
        {
            gameObject.SetActive(true);
            hp = StartHP;
            pathNodes = new Stack<PathNode>();
            lastCollidingObject = new List<IDamageable>();
            transform.position = startPosition;
            transform.rotation = Quaternion.identity;
        }


        public void SetPath(Stack<PathNode> pathNodes)
        {
            this.pathNodes = pathNodes;
        }

        public void Select()
        {
            selectedIndicator.SetActive(true);
            isSelected = true;
            OnSelected?.Invoke(this, new OnSelectedEventArgs
            {
                name = agentName,
                hp = hp
            });
        }

        public void Deselect()
        {
            selectedIndicator.SetActive(false);
            isSelected = false;
            OnDeselected?.Invoke(this, EventArgs.Empty);
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

        private void FixedUpdate()
        {
            if (!isSelected) return;

            DetectCollisions();
        }

        private void MoveTowardsDestination(Vector3 destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        }

        private void RotateTowardsDirection(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        private void DetectCollisions()
        {
            Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
            Physics.OverlapBoxNonAlloc(transform.position, halfExtents, detectColiders, transform.rotation);
            List<IDamageable> detectObject = new List<IDamageable>();

            foreach(var colider in detectColiders)
            {
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
        
        public void TakeDamage(int damage)
        {
            hp -= damage;

            if (isSelected)
                OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs
                {
                    hp = hp,
                });

            if (hp <= 0)
                DestroySelf();
        }

        public void DestroySelf()
        {
            Deselect();
            OnDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}

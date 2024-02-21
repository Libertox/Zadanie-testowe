using System;
using System.Collections.Generic;
using UnityEngine;
using TestTask.PathFinding;

namespace TestTask
{
    public class Agent : MonoBehaviour, ISelectable, IDamageable
    {
        public static event EventHandler<OnSelectedEventArgs> OnSelected;
        public static event EventHandler OnDeselected;

        public class OnSelectedEventArgs: EventArgs
        {
            public string name;
            public int hp;
        }

        public int damage { get; set; } = 1;

        [SerializeField] private float speed;
        [SerializeField] private string agentName;

       
        private int hp = 3;
        private bool isSelected;
    
        private Stack<PathNode> pathNodes = new Stack<PathNode>();

        private Collider[] detectColiders = new Collider[1];
        private List<IDamageable> lastCollidingObject = new List<IDamageable>();

        public void SetPath(Stack<PathNode> pathNodes)
        {
            this.pathNodes = pathNodes;
        }

        public void Select()
        {
            isSelected = true;
            OnSelected?.Invoke(this, new OnSelectedEventArgs
            {
                name = agentName,
                hp = hp
            });
        }

        public void Deselect()
        {
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

            if (hp <= 0)
                DestroySelf();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}

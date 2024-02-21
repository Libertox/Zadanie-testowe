using System;
using System.Collections.Generic;
using UnityEngine;
using TestTask.PathFinding;
using UnityEngine.InputSystem.XR;

namespace TestTask
{
    public class Agent : MonoBehaviour, ISelectable
    {
        public static event EventHandler<OnSelectedEventArgs> OnSelected;
        public static event EventHandler OnDeselected;

        public class OnSelectedEventArgs: EventArgs
        {
            public string name;
            public int hp;
        }

        [SerializeField] private float speed;

        [SerializeField] private string agentName;

        private int hp;

        private Stack<PathNode> pathNodes = new Stack<PathNode>();

        public void SetPath(Stack<PathNode> pathNodes)
        {
            this.pathNodes = pathNodes;
        }

        public void Select()
        {
            Debug.Log("Select");
            OnSelected?.Invoke(this, new OnSelectedEventArgs
            {
                name = agentName,
                hp = hp
            });
        }

        public void Deselect()
        {
            Debug.Log("Deselect");
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

        private void MoveTowardsDestination(Vector3 destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);
        }

        private void RotateTowardsDirection(Vector3 direction)
        {
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}

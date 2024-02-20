using System;
using System.Collections.Generic;
using UnityEngine;

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

       
    }
}

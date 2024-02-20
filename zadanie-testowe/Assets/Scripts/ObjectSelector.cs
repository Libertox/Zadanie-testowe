using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestTask
{
    public class ObjectSelector: MonoBehaviour
    {
        private void Start()
        {
            GameInput.Instance.OnMouseClicked += GameInput_OnMouseClicked;
        }

        private void GameInput_OnMouseClicked(object sender, GameInput.OnMouseClickedEventArgs e)
        {
            Ray ray = Camera.main.ScreenPointToRay(e.mousePosition);
            float maxDistance = 100f;
            if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance))
            {
                if(raycastHit.transform.TryGetComponent(out ISelectable selectable))
                {
                    selectable.Select();
                }
            }

        }
    }
}

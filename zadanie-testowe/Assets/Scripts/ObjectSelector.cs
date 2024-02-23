using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestTask
{
    public class ObjectSelector: MonoBehaviour
    {
        [SerializeField] private LayerMask selectableObjectLayerMask;

        private Agent selectAgent;

        private void Start()
        {
            GameInput.Instance.OnMouseClicked += GameInput_OnMouseClicked;
        }

        private void GameInput_OnMouseClicked(object sender, GameInput.OnMouseClickedEventArgs e)
        {
            Ray ray = Camera.main.ScreenPointToRay(e.mousePosition);
            float maxDistance = 100f;

            if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, selectableObjectLayerMask))
            {
                HandleSelectableClick(raycastHit);

                if (selectAgent != null)
                    selectAgent.SetPath(MapManager.Instance.GetShortPath(selectAgent.transform.position, raycastHit.point));
            }
            else
            {
                ClearSelectAgent();
            }

        }

        private void HandleSelectableClick(RaycastHit raycastHit)
        {
            if (raycastHit.transform.TryGetComponent(out ISelectable selectable))
            {
                ClearSelectAgent();

                selectable.Select();
                selectAgent = selectable as Agent;
            }
        }

        private void ClearSelectAgent()
        {
            if (selectAgent != null) selectAgent.Deselect();

            selectAgent = null;
        }
    }
}

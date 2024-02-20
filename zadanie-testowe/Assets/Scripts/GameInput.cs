using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestTask
{
    public class GameInput: MonoBehaviour
    {
        public static GameInput Instance { get; private set; }

        public event EventHandler<OnMouseClickedEventArgs> OnMouseClicked;

        public class OnMouseClickedEventArgs: EventArgs
        {
            public Vector2 mousePosition;
        }

        private PlayerInput playerInput;

        private void Awake()
        {
            if(!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            playerInput = new PlayerInput();

            playerInput.Enable();

            playerInput.Player.MouseLeftClick.performed += MouseLeftClick_performed;
        }

        private void MouseLeftClick_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnMouseClicked?.Invoke(this, new OnMouseClickedEventArgs
            {
                mousePosition = GetMousePosition()
            });
        }

        private Vector2 GetMousePosition()
        {
            return playerInput.Player.MousePosition.ReadValue<Vector2>();
        }

        private void OnDestroy()
        {
            playerInput.Player.MouseLeftClick.performed-= MouseLeftClick_performed;

            playerInput.Dispose();

        }

    }
}

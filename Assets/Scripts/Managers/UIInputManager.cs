using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheNecromancers.InputManagers
{
    public class UIInputManager : MonoBehaviour, Controls.IUIActions
    {
        public event Action OpenInventory;
        public event Action OpenPauseMenu;

        private Controls controls;

        private void Start()
        {
            controls = new Controls();

            controls.UI.SetCallbacks(this);
            controls.UI.Enable();
        }

        public void OnPauseMenu(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }

            OpenPauseMenu?.Invoke();
        }

        public void OnToggleInventory(InputAction.CallbackContext context)
        {

            if (!context.performed) { return; }

            OpenInventory?.Invoke();
        }
    }
}

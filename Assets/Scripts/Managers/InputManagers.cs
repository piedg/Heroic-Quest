using UnityEngine;
using UnityEngine.InputSystem;

namespace TheNecromancers.Managers
{
    public class InputManager : MonoBehaviour, Controls.IPlayerActions
    {
        public Vector2 MovementValue { get; private set; }

        private Controls controls;

        private void Start()
        {
            controls = new Controls();

            controls.Player.SetCallbacks(this);
            controls.Player.Enable();
        }

        private void OnDestroy()
        {
            controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
        }

        public void DisablePlayerControls()
        {
            controls.Player.Disable();
        }

        public void EnablePlayerControls()
        {
            controls.Player.Enable();
        }
    }
}
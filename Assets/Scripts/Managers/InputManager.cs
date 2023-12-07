using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TheNecromancers.Managers
{
    public class InputManager : MonoBehaviour, Controls.IPlayerActions
    {
        public Vector2 MovementValue { get; private set; }
        public bool IsAttacking { get; private set; }

        public event Action RollEvent;
        public event Action BlockEvent;
        public event Action InteractEvent;

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

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                StartCoroutine(HandleAttackClick());
            }
            else if (context.canceled)
            {
                IsAttacking = false;
            }
        }

        IEnumerator HandleAttackClick()
        {
            IsAttacking = true;
            yield return new WaitForSeconds(0.1f);
            IsAttacking = false;
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }

            RollEvent?.Invoke();
        }

        public void OnBlock(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }

            BlockEvent?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            InteractEvent?.Invoke();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SantaVandal.FinalCharacterController 
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        #region Class Variables
        [SerializeField] private bool holdToSprint = true;
        // public PlayerControls PlayerControls { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool SprintToggleOn { get; private set; }
        public bool WalkToggleOn { get; private set; }
        #endregion

        #region Startup
        private void OnEnable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("PlayerControls is not initialized - cannot enable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Enable();
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("PlayerControls is not initialized - cannot disable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.Disable();
            PlayerInputManager.Instance.PlayerControls.PlayerLocomotionMap.RemoveCallbacks(this);
        }
        #endregion

        #region Late Update Logic
        private void LateUpdate()
        {
            JumpPressed = false;
        }
        #endregion

        #region Input Callback
        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
            Debug.Log(MovementInput);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }

        public void OnToggleSpint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SprintToggleOn = holdToSprint || !SprintToggleOn;
            }
            else if (context.canceled)
            {
                SprintToggleOn = !holdToSprint && SprintToggleOn;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            JumpPressed = true;
        }

        public void OnToggleWalk(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            WalkToggleOn = !WalkToggleOn;
        }
        #endregion
    }
}

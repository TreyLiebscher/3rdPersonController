using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SantaVandal.FinalCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class ThirdPersonInput : MonoBehaviour, PlayerControls.IThirdPersonMapActions
    {
        #region Class Variables
        public Vector2 ScrollInput { get; private set; }

        public Vector3 CamOffset { get; private set; }

        [SerializeField] public CinemachineThirdPersonFollow aimCam;
        [SerializeField] private float _cameraZoomSpeed = 0.7f;
        [SerializeField] private float _cameraMinZoom = 1f;
        [SerializeField] private float _cameraMaxZoom = 5f;

        private CinemachineThirdPersonFollow _thirdPersonFollow;
        #endregion

        #region Startup
        private void Awake()
        {
            _thirdPersonFollow = aimCam.GetComponent<CinemachineThirdPersonFollow>();
        }
        private void OnEnable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot enable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.Enable();
            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.SetCallbacks(this);
        }

        private void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot disable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.Disable();
            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.RemoveCallbacks(this);
        }
        #endregion

        #region Update
        private void Update()
        {

        }

        private void LateUpdate()
        {
            _thirdPersonFollow.CameraDistance = Mathf.Clamp(_thirdPersonFollow.CameraDistance + ScrollInput.y, _cameraMinZoom, _cameraMaxZoom);

            // find the optimal height for the camera after zoom
            float kiwiDistance = _thirdPersonFollow.ShoulderOffset.y + ScrollInput.y / _cameraMaxZoom;
            // move the camera to the side of the player when closer
            float shoulderSideOffset = Math.Abs(kiwiDistance / 2);

            if (_thirdPersonFollow.CameraDistance < _cameraMaxZoom && _thirdPersonFollow.CameraDistance > _cameraMinZoom)
            {
                _thirdPersonFollow.ShoulderOffset = new Vector3(shoulderSideOffset, kiwiDistance, _thirdPersonFollow.ShoulderOffset.z);
            }

            ScrollInput = Vector2.zero;
        }
        #endregion

        #region Input Callbacks
        public void OnScrollCamera(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            Vector2 scrollInput = context.ReadValue<Vector2>();
            ScrollInput = -1f * scrollInput.normalized * _cameraZoomSpeed;
        }
        #endregion
    }
}

using System;
using Client_Side;
using Fusion;
using UnityEngine;

namespace Server_Side
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private Transform fppCameraTarget;


        [SerializeField] private PlayerInputHandler inputHandler;
        public NetworkPlayerInput Inputs => inputHandler.Inputs;


        private void Start()
        {
            SetFppCamera();
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            Cursor.lockState = hasFocus ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void SetFppCamera()
        {
            if (!Object.HasInputAuthority) return;
            
            PlayerFppCamera.SetTarget(fppCameraTarget);
        }

        public void TiltFppCamera(float pitch)
        {
            var oldRotation = fppCameraTarget.eulerAngles;

            oldRotation.x = pitch;

            fppCameraTarget.eulerAngles = oldRotation;
        }
    }
}
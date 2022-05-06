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
            PlayerFppCamera.SetTarget(fppCameraTarget);
        }
    }
}
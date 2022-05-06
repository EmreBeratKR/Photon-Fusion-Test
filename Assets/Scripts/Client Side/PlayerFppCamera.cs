using System;
using Helpers;
using UnityEngine;
using Cinemachine;

namespace Client_Side
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PlayerFppCamera : Scenegleton<PlayerFppCamera>
    {
        private CinemachineVirtualCamera virtualCamera;

        protected override void Awake()
        {
            base.Awake();
            virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
        }


        public static void SetTarget(Transform target)
        {
            Instance.virtualCamera.Follow = target;
        }
    }
}

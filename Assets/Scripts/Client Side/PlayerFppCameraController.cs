using Fusion;
using UnityEngine;
using NetworkPlayer = Server_Side.NetworkPlayer;

namespace Client_Side
{
    public class PlayerFppCameraController : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField] private NetworkPlayer networkPlayer;
        [SerializeField] private PlayerInputHandler inputHandler;
        [SerializeField] private Transform fppCameraTarget;
        [SerializeField] private Transform mainBody;

        [Header("Values")]
        [SerializeField, Range(0, 90)] private float topMaxPitch;
        [SerializeField, Range(0, 90)] private float bottomMaxPitch;
        [SerializeField] private float mouseSensivity;


        public float Angle => mainBody.eulerAngles.y;
        
        private float pitch;
        private void Update()
        {
            if (!networkPlayer.HasInputAuthority) return;   

            var mouseDelta = inputHandler.MouseDelta;
            
            mainBody.Rotate(Vector3.up * (mouseDelta.x * mouseSensivity));
            
            pitch -= mouseDelta.y * mouseSensivity;

            pitch = Mathf.Clamp(pitch, -topMaxPitch, bottomMaxPitch);

            var oldRotation = fppCameraTarget.eulerAngles;

            oldRotation.x = pitch;

            fppCameraTarget.eulerAngles = oldRotation;

            RpcSyncBodyRotation(this.Object.Id.Raw, pitch, mainBody.transform.eulerAngles.y);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        public void RpcSyncBodyRotation(uint objectId, float newPitch, float newAngle)
        {
            if (this.networkPlayer.HasInputAuthority) return;
            
            if (this.Object.Id.Raw != objectId) return;

            var temp = fppCameraTarget.eulerAngles;
            temp.x = newPitch;
            fppCameraTarget.eulerAngles = temp;
            
            temp = mainBody.eulerAngles;
            temp.y = newAngle;
            mainBody.eulerAngles = temp;
        }
    }
}

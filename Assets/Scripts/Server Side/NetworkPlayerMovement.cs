using System;
using Fusion;
using UnityEngine;
using UnityEngine.Events;

namespace Server_Side
{
    [RequireComponent(typeof(Rigidbody), typeof(NetworkRigidbody))]
    public class NetworkPlayerMovement : NetworkBehaviour
    {
        private static float BaseGravityForce = 9.81f;
        
        [Header("References")]
        [SerializeField] private NetworkPlayer networkPlayer;
        
        [Header("Physic")]
        [SerializeField] private PhysicMaterial movementPhysic;
        [SerializeField] private Collider[] colliders;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float gravityScale;
        private Rigidbody body;

        [Header("Speed Values")]
        [SerializeField] private float accelerationRate;
        [SerializeField] private float deaccelerationRate;
        [SerializeField] private float speed;
        [SerializeField] private float terminalSpeed;
        [SerializeField] private float mouseSensivity;

        [Space(10)]
        [SerializeField] private Callbacks callbacks;

        private NetworkPlayerInput input;
        private float pitch;
        

        private Vector3 DesiredVelocity
        {
            get
            {
                var result = Vector3.zero;

                if (input.HasInput)
                {
                    if (input.Check(InputFlag.Forward))
                    {
                        result += Vector3.forward;
                    }
                    
                    if (input.Check(InputFlag.Back))
                    {
                        result += Vector3.back;
                    }
                    
                    if (input.Check(InputFlag.Right))
                    {
                        result += Vector3.right;
                    }
                    
                    if (input.Check(InputFlag.Left))
                    {
                        result += Vector3.left;
                    }
                }

                return result.normalized * speed;
            }
        }

        private float Gravity => BaseGravityForce * gravityScale;

        private float JumpVelocity => Mathf.Sqrt(jumpHeight * 2f * Gravity);
        
        private bool IsReachTerminalSpeed => body.velocity.y < 0 && Mathf.Abs(body.velocity.y) >= terminalSpeed;
        
        private void Awake()
        {
            body = this.GetComponent<Rigidbody>();
            body.useGravity = false;
        }


        private void CacheInput()
        {
            if (GetInput(out input)) return;

            input.Reset();
        }
        
        private void TogglePhysicMaterial(bool enable)
        {
            foreach (var collider in colliders)
            {
                collider.material = enable ? movementPhysic : null;
            }
        }

        private void Move()
        {
            TogglePhysicMaterial(true);
            
            var localDesiredVelocity = (transform.rotation * DesiredVelocity);

            var velocityChange = (localDesiredVelocity - body.velocity) * accelerationRate;
            velocityChange.y = 0;

            body.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        private void Stop()
        {
            TogglePhysicMaterial(false);

            var velocityChange = (body.velocity * -deaccelerationRate);
            velocityChange.y = 0;

            body.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        private void HandleMovement()
        {
            if (DesiredVelocity == Vector3.zero)
            {
                Stop();
                return;
            }
            
            Move();
        }

        private bool TryJump()
        {
            if (!input.Check(InputFlag.Jump)) return false;
            
            body.AddForce(Vector3.up * JumpVelocity, ForceMode.VelocityChange);
            
            callbacks.onJump?.Invoke();
            return true;
        }

        private void ApplyGravity()
        {
            if (IsReachTerminalSpeed) return;
            
            body.AddForce(Vector3.down * BaseGravityForce * gravityScale, ForceMode.Acceleration);
        }

        private void LookAround()
        {
            body.transform.Rotate(Vector3.up * input.LookInput.x * mouseSensivity);

            pitch -= input.LookInput.y * mouseSensivity;

            pitch = Mathf.Clamp(pitch, -90f, 90f);
            
            networkPlayer.TiltFppCamera(pitch);
        }


        public override void FixedUpdateNetwork()
        {
            CacheInput();
            
            HandleMovement();

            TryJump();
            
            ApplyGravity();

            LookAround();
        }
    }

    [Serializable]
    internal struct Callbacks
    {
        public UnityEvent onJump;
    }
}
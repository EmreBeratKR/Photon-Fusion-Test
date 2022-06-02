using Server_Side;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Client_Side
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private Vector2 mouseDelta;
        
        private bool isForward;
        private bool isBack;
        private bool isRight;
        private bool isLeft;

        private bool isJump;
        private bool isSprint;
        
        public NetworkPlayerInput Buttons
        {
            get
            {
                InputFlag inputs = 0;
            
                if (isForward) inputs |= InputFlag.Forward;
            
                if (isBack) inputs |= InputFlag.Back;
            
                if (isRight) inputs |= InputFlag.Right;
            
                if (isLeft) inputs |= InputFlag.Left;

                if (isJump) inputs |= InputFlag.Jump;

                if (isSprint) inputs |= InputFlag.Sprint;

                return new NetworkPlayerInput(inputs);
            }
        }

        public Vector2 MouseDelta => mouseDelta;

        
        public void OnMove(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();

            isForward = value.y > 0;
            isBack = value.y < 0;
            isRight = value.x > 0;
            isLeft = value.x < 0;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            isJump = context.action.IsPressed();
        }

        public void ReleaseJump()
        {
            isJump = false;
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            isSprint = context.action.IsPressed();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            var rawInput = context.ReadValue<Vector2>();
            mouseDelta = rawInput;
        }
    }
}

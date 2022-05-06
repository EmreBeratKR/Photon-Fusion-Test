using System;
using Fusion;
using UnityEngine;

namespace Server_Side
{
    [Flags]
    public enum InputFlag
    {
        Forward = 1 << 0,
        Back = 1 << 1,
        Right = 1 << 2,
        Left = 1 << 3,
        
        Jump = 1 << 4
    }
    
    public struct NetworkPlayerInput : INetworkInput
    {
        private InputFlag inputs;
        public InputFlag Inputs => this.inputs;

        private Vector2 lookInput;
        public Vector2 LookInput => this.lookInput;


        public bool HasInput => inputs != 0;
        
        public NetworkPlayerInput(InputFlag inputs, Vector2 lookInput)
        {
            this.inputs = inputs;
            this.lookInput = lookInput;
        }

        public bool Check(InputFlag input)
        {
            return (input & this.inputs) == input;
        }

        public void Reset()
        {
            inputs = 0;
        }
    }
}
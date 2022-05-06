using System;
using Fusion;

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


        public bool HasInput => inputs != 0;
        
        public NetworkPlayerInput(InputFlag inputs)
        {
            this.inputs = inputs;
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
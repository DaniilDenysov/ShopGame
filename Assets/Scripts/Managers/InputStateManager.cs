using ShopGame.Utilities;
using System;
using System.Collections.Generic;

namespace ShopGame.Managers
{
    public class InputStateManager
    {
        private PlayerInputActions inputActions = new PlayerInputActions();
        public PlayerInputActions InputActions
        {
            get => inputActions;
        }
        private Dictionary<Type, IPlayerInputState> inputMapping = new();
        private IPlayerInputState currentState;

        public InputStateManager()
        {
           
        }

        public void ChangeState(IPlayerInputState playerInputState)
        {
            playerInputState.InputActions = inputActions;
            DebugUtility.PrintLine($"Exiting {currentState?.GetType()}...");
            currentState?.Exit();
            currentState = playerInputState;
            DebugUtility.PrintLine($"Entering {currentState?.GetType()}...");
            currentState.Enter();
        }
    }


    public interface IPlayerInputState
    {
        public PlayerInputActions InputActions { get; set; }
        void Enter();
        void Exit();
    }
}

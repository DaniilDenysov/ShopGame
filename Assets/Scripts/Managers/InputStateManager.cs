using ShopGame.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

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
            ChangeState(new DefaultState());
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

    public class DefaultState : IPlayerInputState
    {
        public PlayerInputActions InputActions { get; set; }

        public void Enter()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InputActions.Player.Enable();
        }
        public void Exit()
        {
            InputActions.Player.Disable();
        }

    }

    public class InventoryState : IPlayerInputState
    {
        public PlayerInputActions InputActions { get; set; }

        public void Enter()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            InputActions.Player.Disable();
        }
        public void Exit()
        {
            InputActions.Player.Enable();

        }
    }
}

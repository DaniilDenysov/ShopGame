using ShopGame.Managers;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace ShopGame.UIScreens
{
    public class PauseUIScreen : GameStateUIScreen
    {
        private void OnEnable()
        {
            inputStateManager.InputActions.Player.Pause.performed += OnPaused;
        }

        private void OnPaused(InputAction.CallbackContext context)
        {
            stateManager.ChangeState(this);
        }

        private void OnDisable()
        {
            inputStateManager.InputActions.Player.Pause.performed -= OnPaused;
        }

        public override void Enter()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            InputActions.Player.Move.Disable();
            InputActions.Player.Jump.Disable();
            InputActions.Player.Look.Disable();
            InputActions.Player.Interaction.Disable();
        }

        public override void Exit()
        {
            InputActions.Player.Move.Enable();
            InputActions.Player.Jump.Enable();
            InputActions.Player.Look.Enable();
            InputActions.Player.Interaction.Enable();
        }

        public void Resume()
        {
            stateManager.ExitState(this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            inputStateManager.ChangeState(this);
        }
    }
}

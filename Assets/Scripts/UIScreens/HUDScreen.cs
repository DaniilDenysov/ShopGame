using ShopGame.Managers;
using UnityEngine;
using Zenject;

namespace ShopGame.UIScreens
{
    public class HUDScreen : UIScreen
    {
    

        public override void Enter()
        {
            InputActions.Player.Enable();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            InputActions.Player.Move.Enable();
            InputActions.Player.Jump.Enable();
            InputActions.Player.Look.Enable();
            InputActions.Player.Interaction.Enable();
            InputActions.Player.Pause.Enable();
        }

        public override void Exit()
        {
            InputActions.Player.Disable();
        }


        private void Start()
        {
            stateManager.ChangeState(this);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            inputStateManager.ChangeState(this);
        }
    }
}

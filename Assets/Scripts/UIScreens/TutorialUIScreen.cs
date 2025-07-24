using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShopGame.UIScreens
{
    public class TutorialUIScreen : UIScreen
    {
        public override void Enter()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            InputActions.Player.Disable();
        }

        public override void Exit()
        {
            InputActions.Player.Enable();
        }

        private void Start()
        {
            stateManager.ChangeState(this);
        }

        public void Close()
        {
            stateManager.ExitState(this);
        }
    }
}

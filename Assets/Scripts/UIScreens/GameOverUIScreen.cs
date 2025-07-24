using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.UIScreens
{
    public class GameOverUIScreen : UIScreen
    {
        public override void Enter()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            InputActions.Player.Disable();
        }

        public override void Exit()
        {
            
        }
    }
}

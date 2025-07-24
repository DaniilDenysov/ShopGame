
using UnityEngine;

namespace ShopGame.UIScreens
{
    public class WinScreen : UIScreen
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

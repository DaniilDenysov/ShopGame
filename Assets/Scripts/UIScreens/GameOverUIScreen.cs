using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.UIScreens
{
    public class GameOverUIScreen : GameStateUIScreen
    {
        public override void Enter()
        {
            InputActions.Player.Disable();
        }

        public override void Exit()
        {
            
        }
    }
}

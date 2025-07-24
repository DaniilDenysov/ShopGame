using ShopGame.Managers;
using ShopGame.Utilities;
using UnityEngine;
using Zenject;

namespace ShopGame.UIScreens
{
    public abstract class UIScreen : MonoBehaviour, IUIState, IPlayerInputState
    {
        [SerializeField] protected UITweener screenTweener;

        protected UIStateManager stateManager;
        protected InputStateManager inputStateManager;

        public PlayerInputActions InputActions { get; set; }

        public abstract void Enter();
        public abstract void Exit();

        [Inject]
        private void Construct(UIStateManager stateManager, InputStateManager inputStateManager)
        {
            this.inputStateManager = inputStateManager;
            this.stateManager = stateManager;
        }

        public virtual void OnEnter()
        {
            screenTweener.SetActive(true);
            DebugUtility.PrintLine($"true");
        }

        public virtual void OnExit()
        {
            screenTweener.SetActive(false);
            DebugUtility.PrintLine($"false");
        }
    }
}

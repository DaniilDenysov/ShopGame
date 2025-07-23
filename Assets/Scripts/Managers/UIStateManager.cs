using ShopGame.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace ShopGame.Managers
{
    public interface IUIState
    {
        void OnEnter();
        void OnExit();
    }

    public class UIStateManager
    {
        private readonly Stack<IUIState> stateStack = new Stack<IUIState>();
        public IUIState CurrentState { get; private set; }

        public void ChangeState(IUIState newState)
        {
            if (newState == CurrentState) return;

            DebugUtility.PrintLine($"Exiting state: {CurrentState}");
            CurrentState?.OnExit();

            if (CurrentState != null)
                stateStack.Push(CurrentState);

            CurrentState = newState;

            DebugUtility.PrintLine($"Entered state: {newState}");
            CurrentState?.OnEnter();
        }

        public void ExitState(IUIState state)
        {
            if (state == null || CurrentState != state) return;

            DebugUtility.PrintLine($"Exiting state: {state}");
            state.OnExit();

            CurrentState = stateStack.Count > 0 ? stateStack.Pop() : null;

            if (CurrentState != null)
            {
                DebugUtility.PrintLine($"Resuming previous state: {CurrentState}");
                CurrentState.OnEnter();
            }
            else
            {
                DebugUtility.PrintLine("No previous state to return to.");
            }
        }
    }

}

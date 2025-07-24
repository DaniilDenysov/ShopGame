using ShopGame.Player.Hunger;
using ShopGame.UIScreens;
using ShopGame.Utilities;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace ShopGame.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Range(0,86400)] private float timeEstimation = 1440f;
        [SerializeField] private TMP_Text clockDisplay;
        [SerializeField] private GameOverUIScreen gameOverUIScreen;
        [SerializeField] private WinScreen winUIScreen;
        private Clock clock;
        private UIStateManager stateManager;

        [Inject]
        private void Construct(UIStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        private EventBinding<OnPlayerDied> eventBinding;

        private void OnEnable()
        {
            eventBinding = new EventBinding<OnPlayerDied>(OnPlayerDied);
            EventBus<OnPlayerDied>.Register(eventBinding);
        }

        private void OnPlayerDied()
        {
            clock?.Stop();
        }

        private void OnDisable()
        {
            EventBus<OnPlayerDied>.Deregister(eventBinding);
        }

        public void StartGame()
        {
            clock = new Clock(OnTimeChanged,
                () => 
                {
                    stateManager.ChangeState(gameOverUIScreen);
                    clock = null;
                },
                () => 
                {
                    stateManager.ChangeState(winUIScreen);
                    clock = null;
                });
            clock.Start(timeEstimation);
        }

        private void OnTimeChanged(float obj)
        {
            clockDisplay.text = clock.ToString();
        }

        private void FixedUpdate()
        {
            clock?.Tick(Time.fixedDeltaTime);
        }
    }
}

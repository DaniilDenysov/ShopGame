using ShopGame.Presenters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Player.Hunger
{
    public struct OnPlayerDied : IEvent
    {

    }

    public class PlayerHunger : MonoBehaviour
    {
        [SerializeField] private Slider hunger;
        public int MovementMultiplier = 1;
        private float currentHunger
        {
            get => hunger.value;
            set
            {
                hunger.value = value;
            }
        }
        [SerializeField] private float maxHunger;
        [SerializeField, TooltipAttribute("Points per minute")] private float hungerSpeed = 25f;

        private EventBinding<OnFoodConsumed> eventBinding;
        private void OnEnable()
        {
            eventBinding = new EventBinding<OnFoodConsumed>(Eat);
            EventBus<OnFoodConsumed>.Register(eventBinding);
        }

        private void Eat(OnFoodConsumed consumed)
        {
            if (consumed.Amount == 0) return;
            currentHunger += consumed.ItemSO.HungerPoints * consumed.Amount;
        }

        private void OnDisable()
        {
            EventBus<OnFoodConsumed>.Deregister(eventBinding);
        }


        private IEnumerator Start()
        {
            currentHunger = maxHunger;

            float hungerPerSecond = hungerSpeed / 60f;
            while (true)
            {
                currentHunger = Mathf.Max(0, currentHunger - (MovementMultiplier * (hungerPerSecond * Time.deltaTime)));
                if (currentHunger == 0)
                {
                    EventBus<OnPlayerDied>.Raise(new OnPlayerDied());
                    break;
                }
                yield return null;
            }
        }
    }
}

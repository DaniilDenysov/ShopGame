using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShopGame.Player.Hunger
{
    public class PlayerHunger : MonoBehaviour
    {
        [SerializeField] private Slider hunger;
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

        private IEnumerator Start()
        {
            currentHunger = maxHunger;

            float hungerPerSecond = hungerSpeed / 60f;
            while (true)
            {
                currentHunger = Mathf.Max(0, currentHunger - hungerPerSecond * Time.deltaTime);
                yield return null;
            }
        }

        public void Eat(float amount)
        {
            if (amount < 0) return;
            currentHunger -= amount;
        }
    }
}

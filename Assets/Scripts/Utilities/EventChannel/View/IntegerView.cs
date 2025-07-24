using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace ShopGame.EventChannel.View
{
    public class IntegerView : EventChannelViewBase<int>
    {
        [SerializeField] private TMP_Text integetDisplay;
        [SerializeField] private string preffix, suffix;

        private void Start()
        {
            UpdateView(eventChannel.Value);
        }

        protected override void OnEventChannelUpdate(int arg)
        {
            UpdateView(arg);
        }

        private void UpdateView(int amount)
        {
            integetDisplay.text = $"{preffix}{amount}{suffix}";
        }
    }
}

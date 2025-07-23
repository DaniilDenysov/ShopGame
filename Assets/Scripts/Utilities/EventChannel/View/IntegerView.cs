using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ShopGame.EventChannel.View
{
    public class IntegerView : EventChannelViewBase<int>
    {
        [SerializeField] private TMP_Text integetDisplay;
        [SerializeField] private string preffix, suffix;

        protected override void OnEventChannelUpdate(int arg)
        {
            integetDisplay.text = $"{preffix}{arg}{suffix}";
        }
    }
}

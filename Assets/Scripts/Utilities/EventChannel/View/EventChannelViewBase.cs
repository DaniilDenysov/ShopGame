using ShopGame.EventChannel.ModelPresenter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ShopGame.EventChannel.View
{
    public abstract class EventChannelViewBase<T> : MonoBehaviour
    {
        [SerializeField] protected EventChannel<T> eventChannel;

        protected void OnEnable()
        {
            eventChannel.Register(OnEventChannelUpdate);
        }

        protected abstract void OnEventChannelUpdate(T arg);

        protected void OnDisable()
        {
            eventChannel.Deregister(OnEventChannelUpdate);
        }
    }
}

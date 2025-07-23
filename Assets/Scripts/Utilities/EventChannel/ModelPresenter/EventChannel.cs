using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShopGame.EventChannel.ModelPresenter
{
    public abstract class EventChannel<T> : ScriptableObject
    {
        protected Action<T> @event;

        [SerializeField] private T initialValue;
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    @event?.Invoke(_value);
                }
            }
        }

        private void OnEnable()
        {
            _value = initialValue;
        }

        private void Awake()
        {
            Value = initialValue;
        }

        public void Invoke(T arg)
        {
            @event?.Invoke(arg);
        }

        public void Register(Action<T> func)
        {
            @event += func;
        }

        public void Deregister(Action<T> func)
        {
            @event -= func;
        }
    }
}

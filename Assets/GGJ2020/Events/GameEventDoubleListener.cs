namespace GGJ2020.Events
{
    using System;
    using PushForward.EventSystem;
    using UnityEngine;
    using UnityEngine.Events;

    [Serializable] public class DoubleEvent : UnityEvent<double> { }
    public class GameEventDoubleListener : GameEventListenerBase
    {
        [SerializeField] private GameEventDouble gameEventDouble;
        protected override GameEvent GameEvent => this.gameEventDouble;
		
        [SerializeField] private DoubleEvent doubleResponse;

        protected override void OnEventRaised()
        {
            this.doubleResponse?.Invoke(this.gameEventDouble.@double);
        }
    }
}
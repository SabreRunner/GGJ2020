namespace GGJ2020.Events
{
    using System;
    using PushForward.EventSystem;
    using UnityEngine;
    using UnityEngine.Events;

    [Serializable] public class HealthEvent : UnityEvent<GameEventHealth.HealthStruct> {}
    public class GameEventHealthListener : GameEventListenerBase
    {
        [SerializeField] private GameEventHealth gameEventHealth;
        protected override GameEvent GameEvent => this.gameEventHealth;
        [SerializeField] private HealthEvent healthResponse;
        protected override void OnEventRaised()
        {
            this.healthResponse?.Invoke(this.gameEventHealth.healthStruct);
        }
    }
}
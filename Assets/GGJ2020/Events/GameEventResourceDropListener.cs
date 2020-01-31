
namespace GGJ2020.Events
{
    using System;
    using PushForward.EventSystem;
    using UnityEngine;
    using UnityEngine.Events;

    [Serializable] public class ResourceDropEvent : UnityEvent<GameEventResourceDrop.ResourceDrop> { } 
    public class GameEventResourceDropListener: GameEventListenerBase
    {
        [SerializeField] private GameEventResourceDrop gameEventResourceDrop;
        protected override GameEvent GameEvent => this.gameEventResourceDrop;

        public ResourceDropEvent resourceDropResponse;

        protected override void OnEventRaised()
        { this.resourceDropResponse?.Invoke(this.gameEventResourceDrop.resourceDrop); }
    }
}
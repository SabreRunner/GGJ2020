namespace GGJ2020.Events
{
    using PushForward.EventSystem;
    using UnityEngine;
    using UnityEngine.Serialization;

    [CreateAssetMenu(menuName = "ScriptableObjects/Game Event Health", order = 121)]
    public class GameEventHealth : GameEvent
    {
        public struct HealthStruct
        {
            public ContinentBlockController continent;
            public float health;
        }
		
        [FormerlySerializedAs("health")] public HealthStruct healthStruct;
		
        public void Raise(HealthStruct newHealthStruct)
        {
            this.healthStruct = newHealthStruct;
            this.Raise();
        }
    }
}
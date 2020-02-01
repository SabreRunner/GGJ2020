namespace GGJ2020.Events
{
    using PushForward.EventSystem;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/Game Event Health", order = 121)]
    public class GameEventHealth : GameEvent
    {
        public struct Health
        {
            public ContinentBlockController continent;
            public float health;
        }
		
        public Health health;
		
        public void Raise(Health newHealth)
        {
            this.health = newHealth;
            this.Raise();
        }
    }
}
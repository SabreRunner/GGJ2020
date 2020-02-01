namespace PushForward.EventSystem
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/Game Event Double", order = 32)]
    public class GameEventDouble : GameEvent
    {
        public double @double;

        public void Raise(double newDouble)
        {
            this.@double = newDouble;
            this.Raise();
        }
    }
}
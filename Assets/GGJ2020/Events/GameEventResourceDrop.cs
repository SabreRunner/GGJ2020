namespace GGJ2020.Events
{
    using PushForward.EventSystem;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/Game Event Resource Drop", order = 122)]
    public class GameEventResourceDrop : GameEvent
    {
        public class ResourceDrop
        {
            public ContinentBlockController.ResourceType resource;
            public ContinentBlockController sourceBlock;
            public ContinentBlockController targetBlock;

            public override string ToString()
                => "ResourceDrop\n Resource: " + this.resource + "; Source:" + this.sourceBlock + "; Target: " + this.targetBlock;
        }

        public ResourceDrop resourceDrop;

        public void Raise(ResourceDrop newResourceDrop)
        {
            this.resourceDrop = newResourceDrop;
            this.Raise();
        }
    }
}
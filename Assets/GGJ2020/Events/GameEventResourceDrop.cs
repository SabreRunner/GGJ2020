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
            public ContinentBlockController targetBlock;
            public Vector3 dropPoint;

            public override string ToString()
                => "ResourceDrop\n Resource: " + this.resource + "; Target: " + this.targetBlock + "; Point: " + this.dropPoint.StringRepresentation();
        }

        public ResourceDrop resourceDrop;

        public void Raise(ResourceDrop newResourceDrop)
        {
            this.resourceDrop = newResourceDrop;
            this.Raise();
        }
    }
}
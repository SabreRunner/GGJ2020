namespace GGJ2020.Events
{
    using PushForward.EventSystem;

    public class GameEventResourceDrop : GameEvent
    {
        public class ResourceDrop
        {
            public ContinentBlockController.ResourceType resource;
            public ContinentBlockController targetBlock;
        }
		
        public ResourceDrop resourceDrop;
		
        public void Raise(ResourceDrop newResourceDrop)
        {
            this.resourceDrop = newResourceDrop;
            this.Raise();
        }
    }
}
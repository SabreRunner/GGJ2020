using GGJ2020.Events;
using UnityEngine;

namespace GGJ2020
{
    public class ContinentBlockController : BaseMonoBehaviour
    {
        public enum ResourceType { None, Trees, Water }
        public enum StatusType { None, Fire, Flood }

        [SerializeField] private ResourceType resource;
        public ResourceType Resource => this.resource;
        [SerializeField] private StatusType status;
        public StatusType Status => this.status;

        public void ResourceGrabbed()
        {
            this.Temp("ResourceGrabbed", "Grabbed " + this.Resource);
        }

        public void ResourceDropped(GameEventResourceDrop.ResourceDrop resourceDrop)
        {
            this.Temp("ResourceDropped", "Dropped: " + resourceDrop);
            if (resourceDrop.targetBlock != this)
            { return; }


        }
    }
}

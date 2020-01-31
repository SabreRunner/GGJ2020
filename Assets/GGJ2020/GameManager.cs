
namespace GGJ2020
{
    using Events;
    using UnityEngine;
    using PushForward.EventSystem;

    public class GameManager : BaseMonoBehaviour
    {
        [SerializeField] GameEventInt resourceGrabbed;
        [SerializeField] GameEventResourceDrop resourceDrop;

        public enum MouseClickMode { Grab, Release }

        public MouseClickMode mouseClickMode;
        public ContinentBlockController.ResourceType grabbedResource;
        
        public void MouseClickRaycast()
        {
            Ray screenPointToRay = this.GetMainCamera().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(screenPointToRay, out RaycastHit hit, 100f))
            {
                ContinentBlockController continent = hit.transform.GetComponent<ContinentBlockController>();
                if (continent == null)
                { return; }
            
                if (this.mouseClickMode == MouseClickMode.Grab)
                {
                    this.grabbedResource = continent.Resource;
                    this.resourceGrabbed?.Raise((int)this.grabbedResource);
                    this.mouseClickMode = MouseClickMode.Release;
                    this.Temp("MouseClickRaycast", "Grabbed " + this.grabbedResource);
                } else if (this.mouseClickMode == MouseClickMode.Release)
                {
                    this.Temp("MouseClickRaycast", "Dropped " + this.grabbedResource);
                    this.resourceDrop?.Raise(new GameEventResourceDrop.ResourceDrop { resource = this.grabbedResource, targetBlock = continent });
                    this.grabbedResource = ContinentBlockController.ResourceType.None;
                    this.mouseClickMode = MouseClickMode.Grab;
                }
            }
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            { this.MouseClickRaycast(); }
        }
    }
}


namespace GGJ2020
{
    using Events;
    using UnityEngine;
    using PushForward.EventSystem;

    public class GameManager : BaseMonoBehaviour
    {
        [SerializeField] GameEventInt resourceGrabbed;
        [SerializeField] GameEventResourceDrop resourceDrop;
        
        private enum MouseClickMode { Grab, Release }
        private MouseClickMode mouseClickMode;
        private ContinentBlockController.ResourceType grabbedResource;
        
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
                } else if (this.mouseClickMode == MouseClickMode.Release)
                {
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


namespace GGJ2020
{
    using System;
    using Events;
    using UnityEngine;
    using PushForward.EventSystem;
    using Random = UnityEngine.Random;

    public class GameManager : SingletonBehaviour<GameManager>
    {
        public GameConfiguration gameConfiguration;
        [SerializeField] private GameEventInt resourceGrabbed;
        [SerializeField] private GameEventResourceDrop resourceDrop;

        public enum MouseClickMode { Grab, Release }
        public MouseClickMode mouseClickMode;
        public ContinentBlockController.ResourceType grabbedResource;
        private bool canClick = true;

        public void MouseClickRaycast()
        {
            Ray screenPointToRay = this.GetMainCamera().ScreenPointToRay(Input.mousePosition);
            
            if (this.canClick && Physics.Raycast(screenPointToRay, out RaycastHit hit, 100f))
            {
                ContinentBlockController continent = hit.transform.GetComponent<ContinentBlockController>();
                if (continent == null)
                { return; }

                if (this.mouseClickMode == MouseClickMode.Grab)
                {
                    continent.ResourceGrabbed();
                    this.grabbedResource = continent.Resource;
                    this.resourceGrabbed?.Raise((int)this.grabbedResource);
                    // this.Temp("MouseClickRaycast", "Grabbed " + this.grabbedResource);
                } else if (this.mouseClickMode == MouseClickMode.Release)
                {
                    // this.Temp("MouseClickRaycast", "Dropped " + this.grabbedResource);
                    GameEventResourceDrop.ResourceDrop drop = new GameEventResourceDrop.ResourceDrop
                                                                  { resource = this.grabbedResource, targetBlock = continent, dropPoint = hit.point};
                    this.resourceDrop?.Raise(drop);
                    continent.ResourceDropped(drop);
                    this.grabbedResource = ContinentBlockController.ResourceType.None;
                }

                this.canClick = false;
                this.ActionInSeconds(()=>this.canClick = true, GameManager.Instance.gameConfiguration.holdTimeStepInSeconds);
            }
        }

        public void Update()
        {
            if (Input.GetMouseButton(0))
            { this.MouseClickRaycast(); }
            
            if (Input.GetMouseButtonUp(0))
            {
                switch (this.mouseClickMode)
                {
                    case MouseClickMode.Grab: this.mouseClickMode = MouseClickMode.Release; break;
                    case MouseClickMode.Release: this.mouseClickMode = MouseClickMode.Grab; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Awake()
        {
            this.SetInstance(this);
        }
    }
}

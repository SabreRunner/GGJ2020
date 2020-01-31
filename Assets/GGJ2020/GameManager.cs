
namespace GGJ2020
{
    using System;
    using Events;
    using UnityEngine;
    using PushForward.EventSystem;
    using Random = UnityEngine.Random;

    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField] private GameEventInt resourceGrabbed;
        [SerializeField] private GameEventResourceDrop resourceDrop;
        [SerializeField] private GameObject firePrefab;
        [SerializeField] private GameObject[] treePrefabs;

        public enum MouseClickMode { Grab, Release }
        public MouseClickMode mouseClickMode;
        public ContinentBlockController.ResourceType grabbedResource;

        public static GameObject FirePrefab => Instance.firePrefab;
        public static GameObject TreePrefab => Instance.treePrefabs[Random.Range(0, Instance.treePrefabs.Length - 1)];

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
                    continent.ResourceGrabbed();
                    this.grabbedResource = continent.Resource;
                    this.resourceGrabbed?.Raise((int)this.grabbedResource);
                    this.mouseClickMode = MouseClickMode.Release;
                    // this.Temp("MouseClickRaycast", "Grabbed " + this.grabbedResource);
                } else if (this.mouseClickMode == MouseClickMode.Release)
                {
                    // this.Temp("MouseClickRaycast", "Dropped " + this.grabbedResource);
                    GameEventResourceDrop.ResourceDrop drop = new GameEventResourceDrop.ResourceDrop
                                                                  { resource = this.grabbedResource, targetBlock = continent, dropPoint = hit.point};
                    this.resourceDrop?.Raise(drop);
                    continent.ResourceDropped(drop);
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

        private void Awake()
        {
            this.SetInstance(this);
        }
    }
}

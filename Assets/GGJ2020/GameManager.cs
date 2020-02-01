
namespace GGJ2020
{
    using System;
    using Events;
    using UnityEngine;
    using PushForward.EventSystem;
    using PushForward.ExtensionMethods;
    using UnityEngine.Serialization;

    public class GameManager : SingletonBehaviour<GameManager>
    {
        public GameConfiguration gameConfiguration;
        [SerializeField] private GameEventResourceDrop resourceDrop;
        [SerializeField] private GameEventDouble grabbingProgression;

        public enum MouseClickMode { None, Grab, Drop }
        public MouseClickMode mouseClickMode;
        public ContinentBlockController lastGrabbedContinent;
        public ContinentBlockController.ResourceType grabbedResource;
        private double grabStarted = double.MinValue;
        private bool isPaused = false;
        public bool IsPaused
        {
            get { return isPaused; }
        }

        private ContinentBlockController Raycast()
        {
            Ray screenPointToRay = this.GetMainCamera().ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(screenPointToRay, out RaycastHit hit, 100f)
                       ? hit.transform.GetComponent<ContinentBlockController>() : null;
        }

        private void Release(ContinentBlockController targetContinent)
        {
            if (this.lastGrabbedContinent == null || this.grabbedResource == ContinentBlockController.ResourceType.None)
            { return; }

            this.Temp("Release", "On " + targetContinent.name);
            targetContinent.ResourceDropped(
                new GameEventResourceDrop.ResourceDrop { resource = this.grabbedResource, targetBlock = targetContinent,
                                                           sourceBlock = this.lastGrabbedContinent });
            this.lastGrabbedContinent = null;
            this.grabbedResource = ContinentBlockController.ResourceType.None;
        }

        private void StartGrab(ContinentBlockController sourceContinent)
        {
            if (sourceContinent.Resource == ContinentBlockController.ResourceType.None)
            { return; }

            this.Temp("StartGrab", "From: " + sourceContinent.name);
            this.lastGrabbedContinent = sourceContinent;
            this.grabbedResource = sourceContinent.Resource;
            this.grabStarted = this.CurrentTimeInMilliseconds;
        }

        private void Grabbing(ContinentBlockController sourceContinent)
        {
            if (this.lastGrabbedContinent == sourceContinent
                && this.CurrentTimeInMilliseconds <= this.grabStarted + this.gameConfiguration.grabTimeInSeconds * 1000)
            {
                double grabProgression = (this.CurrentTimeInMilliseconds - this.grabStarted) / 1000 / this.gameConfiguration.grabTimeInSeconds;
                if (grabProgression.Between(0.5f, 0.52f) || grabProgression.Between(0.98f, 1f))
                { this.Temp("Grabbing", "Progression: " + grabProgression); }
                this.grabbingProgression.Raise(grabProgression);
            }
        }

        private double firstClickFrame = double.MinValue;
        public void Update()
        {
            if (!IsPaused) {
            if (Input.GetMouseButtonDown(0))
            {
                if (this.CurrentTimeInMilliseconds < this.firstClickFrame + this.gameConfiguration.doubleClickWindowInMilliseconds)
                {
                    this.Release(this.Raycast());
                    this.firstClickFrame = double.MinValue;
                }
                else
                {
                    this.firstClickFrame = this.CurrentTimeInMilliseconds;
                    this.ActionInSeconds(()=>this.firstClickFrame = double.MinValue,
                                         (float)this.gameConfiguration.doubleClickWindowInMilliseconds / 1000);
                }
            }

            if (Input.GetMouseButton(0) && this.firstClickFrame < 0)
            {
                if (this.grabStarted < 0)
                { this.StartGrab(this.Raycast()); }
                else { this.Grabbing(this.Raycast()); }
            }

            if (Input.GetMouseButtonUp(0))
            {
                this.grabStarted = double.MinValue;
                if (this.CurrentTimeInMilliseconds > this.firstClickFrame + this.gameConfiguration.doubleClickWindowInMilliseconds)
                { this.firstClickFrame = double.MinValue; }
            }
            }
        }

        private void Awake()
        {
            this.SetInstance(this);
        }

        // TODO: Make this method static.
        public void Pause()
        {
            isPaused = true; // TODO: Create an event for pausing the game.
            Time.timeScale = 0;
        }

        // TODO: Make this method static.
        public void Resume()
        {
            isPaused = false; // TODO: Create an event for resuming the game.
            Time.timeScale = 1;
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

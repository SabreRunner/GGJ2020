
namespace GGJ2020
{
    using System;
    using System.Collections.Generic;
    using Events;
    using UnityEngine;
    using PushForward.EventSystem;
    using PushForward.ExtensionMethods;
    using UnityEngine.Serialization;

    public class GameManager : SingletonBehaviour<GameManager>
    {
        public GameConfiguration gameConfiguration;
        [SerializeField] private GameEventResourceDrop resourceDrop;
        [SerializeField] private GameEventDouble grabbingProgressionEvent;

        public enum MouseClickMode { None, Grab, Drop }
        public MouseClickMode mouseClickMode;
        public ContinentBlockController lastGrabbedContinent;
        public ContinentBlockController.ResourceType grabbedResource;
        private double grabStarted = double.MinValue;
        private double grabProgression;
        private bool isPaused = false;
        public bool IsPaused
        {
            get { return this.isPaused; }
        }

        private ContinentBlockController Raycast()
        {
            Ray screenPointToRay = this.GetMainCamera().ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(screenPointToRay, out RaycastHit hit, 100f)
                       ? hit.transform.GetComponent<ContinentBlockController>() : null;
        }

        private void Release(ContinentBlockController targetContinent)
        {
            // Return if there is not grabbed resource or if the target continent expects a different resource.
            if (this.lastGrabbedContinent == null || this.grabProgression < 0.9f
                || this.grabbedResource == ContinentBlockController.ResourceType.None
                || targetContinent.Resource == ContinentBlockController.ResourceType.Water && this.grabbedResource != ContinentBlockController.ResourceType.Water)
            { return; }

            // this.Temp("Release", "On " + targetContinent.name);
            GameEventResourceDrop.ResourceDrop resourceDropped = new GameEventResourceDrop.ResourceDrop
                                                                     {
                                                                         resource = this.grabbedResource,
                                                                         targetBlock = targetContinent,
                                                                         sourceBlock = this.lastGrabbedContinent
                                                                     };
            this.resourceDrop.Raise(resourceDropped);
            targetContinent.ResourceDropped(resourceDropped);

            this.lastGrabbedContinent = null;
            this.grabbedResource = ContinentBlockController.ResourceType.None;
        }

        private void StartGrab(ContinentBlockController sourceContinent)
        {
            if (sourceContinent.Resource == ContinentBlockController.ResourceType.None)
            { return; }

            // this.Temp("StartGrab", "From: " + sourceContinent.name);
            this.lastGrabbedContinent = sourceContinent;
            this.grabbedResource = sourceContinent.Resource;
            this.grabStarted = this.CurrentTimeInMilliseconds;
        }

        private void Grabbing(ContinentBlockController sourceContinent)
        {
            if (this.lastGrabbedContinent == sourceContinent
                && this.CurrentTimeInMilliseconds <= this.grabStarted + this.gameConfiguration.grabTimeInSeconds * 1000)
            {
                this.grabProgression = (this.CurrentTimeInMilliseconds - this.grabStarted) / 1000 / this.gameConfiguration.grabTimeInSeconds;
                // if (this.grabProgression.Between(0.98f, 1f))
                // { this.Temp("Grabbing", "Progression: " + this.grabProgression); }
                this.grabbingProgressionEvent.Raise(this.grabProgression);
            }
        }
        
        private void CheckMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (this.CurrentTimeInMilliseconds <
                    this.firstClickFrame + this.gameConfiguration.doubleClickWindowInMilliseconds)
                {
                    this.Temp("CheckMouseInput", "Second click of double click." + " (Frame: " + Time.frameCount + ")");
                    this.Release(this.Raycast());
                    return;
                }
                else
                {
                    this.Temp("CheckMouseInput", "First click of double click." + " (Frame: " + Time.frameCount + ")");
                    this.firstClickFrame = this.CurrentTimeInMilliseconds;
                    this.ActionInSeconds(() => this.firstClickFrame = double.MinValue,
                                         (float)this.gameConfiguration.doubleClickWindowInMilliseconds / 1000);
                }
            }

            if (Input.GetMouseButton(0) && this.firstClickFrame < 0)
            {
                this.Temp("CheckMouseInput", "Grabbing." + " (Frame: " + Time.frameCount + ")");
                if (this.grabStarted < 0)
                { this.StartGrab(this.Raycast()); }
                else { this.Grabbing(this.Raycast()); }
            }

            if (Input.GetMouseButtonUp(0))
            {
                this.Temp("CheckMouseInput", "Release." + " (Frame: " + Time.frameCount + ")");
                this.grabStarted = double.MinValue;
                if (this.CurrentTimeInMilliseconds > this.firstClickFrame + this.gameConfiguration.doubleClickWindowInMilliseconds
                    || this.lastGrabbedContinent == null)
                { this.firstClickFrame = double.MinValue; }
            }
        }

        private double firstClickFrame = double.MinValue;
        public void Update()
        {
            if (!this.IsPaused)
            { this.CheckMouseInput(); }
        }

        private void Awake()
        {
            this.SetInstance(this);
        }

        // TODO: Make this method static.
        public void Pause()
        {
            this.isPaused = true; // TODO: Create an event for pausing the game.
            Time.timeScale = 0;
        }

        // TODO: Make this method static.
        public void Resume()
        {
            this.isPaused = false; // TODO: Create an event for resuming the game.
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

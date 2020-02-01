
namespace GGJ2020
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Events;
    using UnityEngine;
    using PushForward.EventSystem;
    using PushForward.ExtensionMethods;

    public class SunController : BaseMonoBehaviour
    {
        [SerializeField] private GameEventBool gameOverEvent;
        [SerializeField] private MeshRenderer sunRenderer;
        [SerializeField] private Material[] sunStates;

        public float averageHealth = 1f;
        private bool canEnd;

        private Dictionary<ContinentBlockController, float> healthDictionary = new Dictionary<ContinentBlockController, float>();

        public void UpdateHealth(GameEventHealth.HealthStruct healthUpdate)
        {
            // this.Temp("UpdateHealth", "Update: " + healthUpdate.continent.name + " - " + healthUpdate.health);
            this.healthDictionary[healthUpdate.continent] = healthUpdate.health;

            float totalHealth = this.healthDictionary.Sum(health => health.Value);
            this.averageHealth = totalHealth / this.healthDictionary.Count;

            // this.Temp("UpdateHealth", "Average Health: " + this.averageHealth + "; Can end = " + this.canEnd);

            int sunState = (this.averageHealth * (this.sunStates.Length - 1)).Floor();

            this.sunRenderer.material = this.sunStates[sunState];

            if (this.canEnd && this.averageHealth <= GameManager.Instance.gameConfiguration.healthFailThreshold)
            {
                // this.Temp("UpdateHealth", "You lose.");
                this.gameOverEvent.Raise(false);
            }

            if (this.canEnd && this.averageHealth > 0.9 && !this.healthDictionary.Any(pair => pair.Key.OnFire))
            {
                // this.Temp("UpdateHealth", "You win.");
                this.gameOverEvent.Raise(true);
            }
        }

        public void StartGame()
        {
            this.ActionInSeconds(()=>this.ActionWhenPredicate(()=> this.canEnd = true, ()=> this.averageHealth < 0.5f), 1f);
        }

        private void OnValidate()
        {
            if (this.sunRenderer == null)
            { this.sunRenderer = this.GetComponentInChildren<MeshRenderer>(); }
        }
    }
}

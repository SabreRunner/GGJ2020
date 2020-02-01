using UnityEngine;

namespace GGJ2020
{
    using System.Collections.Generic;
    using System.Linq;
    using Events;
    using PushForward.ExtensionMethods;

    public class SunController : BaseMonoBehaviour
    {
        [SerializeField] private MeshRenderer sunRenderer;
        [SerializeField] private Material[] sunStates;

        private Dictionary<ContinentBlockController, float> healthDictionary = new Dictionary<ContinentBlockController, float>();

        public void UpdateHealth(GameEventHealth.HealthStruct healthUpdate)
        {
            // this.Temp("UpdateHealth", "Update: " + healthUpdate.continent.name + " - " + healthUpdate.health);
            this.healthDictionary[healthUpdate.continent] = healthUpdate.health;

            float totalHealth = this.healthDictionary.Sum(health => health.Value);
            float averageHealth = totalHealth / this.healthDictionary.Count;

            int sunState = (averageHealth * (this.sunStates.Length - 1)).Floor();

            this.sunRenderer.material = this.sunStates[sunState];
        }

        private void OnValidate()
        {
            if (this.sunRenderer == null)
            { this.sunRenderer = this.GetComponentInChildren<MeshRenderer>(); }
        }
    }
}

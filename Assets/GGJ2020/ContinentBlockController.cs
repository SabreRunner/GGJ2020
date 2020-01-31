namespace GGJ2020
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using PushForward;
    using PushForward.ExtensionMethods;
    using Random = UnityEngine.Random;
    using Events;
    using UnityEngine;

    public class ContinentBlockController : BaseMonoBehaviour
    {
        public enum ResourceType { None, Trees, Water }
        public enum StatusType { None, Fire, Flood }

        [SerializeField] private ResourceType resource;
        public ResourceType Resource => this.resource;
        [SerializeField] private StatusType status;
        public StatusType Status => this.status;
        public float fireRisk;
        [SerializeField] private SpecificInstantiator[] objectInstantiators;
        private List<SpecificInstantiator> TreeInstantiators
        {
            get
            {
                return new List<SpecificInstantiator>(this.objectInstantiators)
                    .FindAll(instantiator => instantiator.spawnType == GameConfiguration.SpawnType.None
                                             || instantiator.spawnType == GameConfiguration.SpawnType.Fire);
            }
        }

        private List<SpecificInstantiator> FireInstantiators
        {
            get
            {
                return new List<SpecificInstantiator>(this.objectInstantiators)
                    .FindAll(instantiator => instantiator.spawnType == GameConfiguration.SpawnType.None
                                             || instantiator.spawnType == GameConfiguration.SpawnType.Tree);
            }
        }

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

        public void CreateRandomFire()
        {
            List<SpecificInstantiator> fireInstantiators = this.FireInstantiators;
            fireInstantiators[Random.Range(0, fireInstantiators.Count - 1)].Instantiate(GameConfiguration.SpawnType.Fire);
        }
        
        private WaitForSeconds fireWaitForSeconds;
        private IEnumerator FireCoroutine()
        {
            while (this.FireInstantiators.Count > 0)
            {
                float roll = Random.Range(0f, 1f);
                // this.Temp("FireCoroutine", "Rolled: " + roll);
                if (roll.Between(0, this.fireRisk))
                { this.CreateRandomFire(); }
                yield return this.fireWaitForSeconds;
                this.fireRisk += GameManager.Instance.gameConfiguration.fireRiskIncrease;
            }
        }
        
        private void StartFirePotential()
        {
            this.fireRisk = GameManager.Instance.gameConfiguration.initialFireRisk;
            this.StartCoroutine(this.FireCoroutine());
        }

        private void Start()
        {
            this.fireWaitForSeconds = new WaitForSeconds(GameManager.Instance.gameConfiguration.fireRiskTimeStepInSeconds);
            this.StartFirePotential();
        }

        private void OnValidate()
        {
            this.objectInstantiators = this.GetComponentsInChildren<SpecificInstantiator>();
        }
    }
}

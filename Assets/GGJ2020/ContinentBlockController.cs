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
        public float health;
        public float fireRisk;
        private Coroutine fireRiskCoroutine;
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

        public void CreateRandomTree()
        {
            List<SpecificInstantiator> treeInstantiators = this.TreeInstantiators;
            if (treeInstantiators.Count == 0)
            { return; }
            treeInstantiators[Random.Range(0, treeInstantiators.Count - 1)].Instantiate(GameConfiguration.SpawnType.Tree);
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

            if (resourceDrop.resource == ResourceType.Trees)
            {
                this.CreateRandomTree();

                this.health = (this.health + GameManager.Instance.gameConfiguration.healthIncreasePerStep).Clamp01();

                if (this.fireRiskCoroutine == null)
                { this.StartFirePotential(); }
            }

            if (resourceDrop.resource == ResourceType.Water)
            {
                this.StopCoroutine(this.fireRiskCoroutine);
            }
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
                this.fireRisk = (this.fireRisk + GameManager.Instance.gameConfiguration.fireRiskIncrease).Clamp01();
            }
        }

        private void StartFirePotential()
        {
            this.fireRisk = GameManager.Instance.gameConfiguration.initialFireRisk.Evaluate(this.health);
            this.fireRiskCoroutine = this.StartCoroutine(this.FireCoroutine());
        }

        private void Start()
        {
            if (this.resource == ResourceType.Trees)
            {
                this.fireWaitForSeconds = new WaitForSeconds(GameManager.Instance.gameConfiguration.fireRiskTimeStepInSeconds);
                this.StartFirePotential();
            }
        }

        private void Awake()
        {
            this.objectInstantiators = this.GetComponentsInChildren<SpecificInstantiator>();
        }
    }
}

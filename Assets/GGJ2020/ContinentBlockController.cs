namespace GGJ2020
{
    using System.Collections;
    using System.Collections.Generic;
    using PushForward.ExtensionMethods;
    using Random = UnityEngine.Random;
    using Events;
    using UnityEngine;

    public class ContinentBlockController : BaseMonoBehaviour
    {
        public enum ResourceType { None, Trees, Water }

        [SerializeField] private Material meshMaterial;
        [SerializeField] private ResourceType resource;
        public ResourceType Resource => this.resource;
        public float health;
        public float fireRisk;
        private Coroutine fireRiskCoroutine;
        [SerializeField] private GameEventHealth healthEvent;
        [SerializeField] private SpecificInstantiator[] objectInstantiators;
        private List<SpecificInstantiator> InstantiatorsWithFire
        {
            get
            {
                return new List<SpecificInstantiator>(this.objectInstantiators)
                    .FindAll(instantiator => instantiator.spawnType == GameConfiguration.SpawnType.Fire);
            }
        }

        private List<SpecificInstantiator> InstantiatorsWithTrees
        {
            get
            {
                return new List<SpecificInstantiator>(this.objectInstantiators)
                    .FindAll(instantiator => instantiator.spawnType == GameConfiguration.SpawnType.Tree);
            }
        }

        private List<SpecificInstantiator> InstantiatorsWithNone
        {
            get
            {
                return new List<SpecificInstantiator>(this.objectInstantiators)
                    .FindAll(instantiator => instantiator.spawnType == GameConfiguration.SpawnType.None);
            }
        }

        private List<SpecificInstantiator> InstantiatorsForFire
        {
            get
            {
                List<SpecificInstantiator> list = this.InstantiatorsWithNone;
                list.AddRange(this.InstantiatorsWithTrees);
                return list;
            }
        }

        public void UpdateHealth()
        {
            this.health = this.InstantiatorsWithTrees.Count / (float)this.objectInstantiators.Length;

            this.meshMaterial.SetFloat("Vector1_DDF08DA6", this.health);
            this.healthEvent?.Raise(new GameEventHealth.HealthStruct { continent = this, health = this.health});
        }

        public bool CreateRandomTree()
        {
            List<SpecificInstantiator> instantiators = this.InstantiatorsWithNone;
            if (instantiators.Count == 0)
            { return true; }
            instantiators[Random.Range(0, instantiators.Count - 1)].Instantiate(GameConfiguration.SpawnType.Tree);
            this.UpdateHealth();
            return false;
        }

        public void CreateTrees()
        {
            if (this.CreateRandomTree())
            { return; }
            this.ActionInSeconds(this.CreateTrees, 0.1f);
        }

        public void ResourceDropped(GameEventResourceDrop.ResourceDrop resourceDrop)
        {
            // this.Temp("ResourceDropped", "Dropped: " + resourceDrop);
            if (resourceDrop.targetBlock != this)
            { return; }

            if (resourceDrop.resource == ResourceType.Trees)
            {
                this.CreateTrees();

                this.resource = ResourceType.Trees;
                this.UpdateHealth();

                if (this.fireRiskCoroutine == null)
                { this.StartFirePotential(); }
            }

            if (resourceDrop.resource == ResourceType.Water)
            {
                if (this.fireRiskCoroutine != null)
                { this.StopCoroutine(this.fireRiskCoroutine); }

                this.InstantiatorsWithFire.DoForEach(inst => inst.Instantiate(GameConfiguration.SpawnType.None));
                this.UpdateHealth();
            }
        }

        public void CreateRandomFire()
        {
            List<SpecificInstantiator> fireInstantiators = this.InstantiatorsForFire;
            fireInstantiators[Random.Range(0, fireInstantiators.Count - 1)].Instantiate(GameConfiguration.SpawnType.Fire);
        }

        private WaitForSeconds fireWaitForSeconds;
        private IEnumerator FireCoroutine()
        {
            while (this.InstantiatorsForFire.Count > 0)
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

            this.objectInstantiators.DoForEach(inst => inst.SetPrefabAndInstantiate());
            this.UpdateHealth();
        }

        private void Awake()
        {
            this.objectInstantiators = this.GetComponentsInChildren<SpecificInstantiator>();
            this.meshMaterial = this.GetComponent<Renderer>().material;
        }
    }
}

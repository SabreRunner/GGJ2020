namespace GGJ2020
{
    using PushForward;
    using UnityEngine;

    class SpecificInstantiator : GenericInstantiator
    {
        public GameConfiguration.SpawnType spawnType;

        public static GameObject FirePrefab => GameManager.Instance.gameConfiguration.firePrefab;
        public static GameObject TreePrefab
            => GameManager.Instance.gameConfiguration.treePrefabs
                [Random.Range(0, GameManager.Instance.gameConfiguration.treePrefabs.Length - 1)];

        public void Instantiate(GameConfiguration.SpawnType spawnType)
        {
            this.DestroyAllChildren();
            this.spawnType = spawnType;

            switch (spawnType)
            {
                case GameConfiguration.SpawnType.Tree: this.Prefab = SpecificInstantiator.TreePrefab; break;
                case GameConfiguration.SpawnType.Fire: this.Prefab = SpecificInstantiator.FirePrefab; break;
                default: this.Prefab = null; break;
            }
			
            this.Instantiate();
        }
    }
}

namespace GGJ2020
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/Game Configuration", order = 1000)]
    public class GameConfiguration : ScriptableObject
    {
        public float sphereDragMultiplier = 1.0f;

        public enum SpawnType { None, Fire, Tree }
        public GameObject firePrefab;
        public GameObject[] treePrefabs;

        public float initialFireRisk = 0.1f;
        public float fireRiskIncrease = 0.1f;
        public float fireRiskTimeStepInSeconds = 5f;
    }
}

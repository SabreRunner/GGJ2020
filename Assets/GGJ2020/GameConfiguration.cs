
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

        public AnimationCurve initialFireRisk;
        public float fireRiskIncrease = 0.1f;
        public float fireRiskTimeStepInSeconds = 5f;

        public long doubleClickWindowInMilliseconds = 200;
        public float grabTimeInSeconds = 3f;
        
        public float healthFailThreshold = 0.09f;
    }
}

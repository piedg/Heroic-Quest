using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectsPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObjects();

            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObjects = GameObject.Instantiate(persistentObjectsPrefab);
            DontDestroyOnLoad(persistentObjects);
        }
    }
}
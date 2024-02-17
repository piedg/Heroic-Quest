using UnityEngine;

namespace HeroicQuest.Data
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/New Enemy")]
    public class EnemySO : ScriptableObject
    {
        [SerializeField] GameObject Model;
        [SerializeField] WeaponSO weaponSO;
        public WeaponSO WeaponSO => weaponSO;

        public void SpawnModel(Transform parent)
        {
            Instantiate(Model, parent);
        }
    }
}

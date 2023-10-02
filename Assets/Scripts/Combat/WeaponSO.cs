using UnityEngine;
using TheNecromancers.Combat;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
public class WeaponSO : ScriptableObject
{
    [field: SerializeField] public GameObject ItemPrefab { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int AnimationLayer { get; private set; }
    [field: SerializeField] public float[] Knockbacks { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    //  [field: SerializeField] public WeaponType WeaponType { get; private set; }

    public void Equip(Transform handHolder)
    {
        Instantiate(ItemPrefab, handHolder);
    }
}

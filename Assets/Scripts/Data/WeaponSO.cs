using UnityEngine;
using TheNecromancers.Combat;

namespace TheNecromancers.Data
{
    public enum eWeaponType
    {
        Unarmed,
        OneHandSword,
        Shield,
        Axe
    }

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
    public class WeaponSO : ItemSO
    {
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public AnimatorOverrideController AnimatorOverride { get; private set; }
        [field: SerializeField] public float[] Knockbacks { get; private set; }
        [field: SerializeField] public Attack[] Attacks { get; private set; }
        [field: SerializeField] public eWeaponType WeaponType { get; private set; }

        public void Equip(Transform handHolder, Animator animator)
        {
            if (itemPrefab)
            {
                Instantiate(itemPrefab, handHolder);
                UpdateAnimator(animator);
            }
        }

        private void UpdateAnimator(Animator animator)
        {
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (AnimatorOverride)
            {
                animator.runtimeAnimatorController = AnimatorOverride;
            }
            else if (overrideController)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }
    }
}

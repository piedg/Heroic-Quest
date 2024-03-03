using HeroicQuest.Gameplay.Combat.Attack;
using UnityEngine;

namespace HeroicQuest.Data
{
    public enum eWeaponType
    {
        Unarmed,
        OneHand,
        TwoHands,
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
                DestroyAlreadyEquippedWeapon(handHolder);

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

        /// <summary>
        /// Check if there is a weapon already instantiated, then destroy it
        /// </summary>
        /// <param name="transform of the hand"></param>
        public void DestroyAlreadyEquippedWeapon(Transform handHolder)
        {
            foreach (Transform children in handHolder)
            {
                if (children.TryGetComponent(out WeaponLogic weaponLogic))
                {
                    if (weaponLogic != null)
                    {
                        Destroy(children.gameObject);
                    }
                }
            }
        }

        public bool IsUnarmed()
        {
            return WeaponType == eWeaponType.Unarmed;
        }

        public bool IsOneHandWeapon()
        {
            return WeaponType == eWeaponType.OneHand;
        }

        public bool IsTwoHandsWeapon()
        {
            return WeaponType == eWeaponType.TwoHands;
        }
    }
}

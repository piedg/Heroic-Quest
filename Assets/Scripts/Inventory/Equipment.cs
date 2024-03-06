using HeroicQuest.Data;
using System.Collections.Generic;
using UnityEngine;

namespace HeroicQuest.Inventory
{
    public class Equipment : MonoBehaviour
    {
        [SerializeField] WeaponSO primaryEquipment;
        [SerializeField] WeaponSO currentPrimaryEquipped = null;

        [SerializeField] WeaponSO secondaryEquipment;
        [SerializeField] List<ItemSO> items;
        [SerializeField] WeaponSO unarmed;

        public WeaponSO PrimaryEquipment { get => primaryEquipment; set => primaryEquipment = value; }
        public WeaponSO CurrentPrimaryEquipped { get => currentPrimaryEquipped; set => currentPrimaryEquipped = value; }

        public WeaponSO SecondaryEquipment { get => secondaryEquipment; set => secondaryEquipment = value; }
        public WeaponSO Unarmed { get => unarmed; }

        public void ToggleEquippedWeapon()
        {
            if (primaryEquipment)
            {
                if (currentPrimaryEquipped == unarmed)
                {
                    currentPrimaryEquipped = primaryEquipment;
                }
                else
                {
                    currentPrimaryEquipped = unarmed;
                }
            }
        }
    }
}
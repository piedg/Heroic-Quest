using HeroicQuest.Data;
using System.Collections.Generic;
using UnityEngine;

namespace HeroicQuest.Inventory
{
    public class Equipment : MonoBehaviour
    {
        [SerializeField] WeaponSO mainEquipment;
        [SerializeField] WeaponSO secondaryEquipment;
        [SerializeField] List<ItemSO> items;

        public WeaponSO MainEquipment { get => mainEquipment; set => mainEquipment = value; }
        public WeaponSO SecondaryEquipment { get => secondaryEquipment; set => secondaryEquipment = value; }
    }
}
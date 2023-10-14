using UnityEngine;

namespace TheNecromancers.Data
{
    public enum eItemType
    {
        None,
        Consumable,
        Equipment
    }

    [CreateAssetMenu(fileName = "Item", menuName = "Items/New Item", order = 0)]
    public class ItemSO : ScriptableObject
    {
        [SerializeField] GameObject inventoryPrefab;
        [SerializeField] protected GameObject itemPrefab;
        [SerializeField] eItemType type;
        [SerializeField] string descriptionTitle;
        [TextArea(15, 20)]
        [SerializeField] string description;
    }
}
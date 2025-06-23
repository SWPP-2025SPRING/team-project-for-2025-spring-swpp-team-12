using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Item", menuName = "Item/Passive")]
public class PassiveItem : Item
{
    public float duration = 5f;

    public override void OnPickup(GameObject player)
    {
        Debug.Log($"[Item] {itemName} OnPickup called");
        Inventory.Instance.AddPassiveItem(this);
    }

    public override void OnUse(GameObject player)
    {
        // PassiveItem은 직접 사용되지 않음
        //Debug.Log($"[WARN] OnUse called on PassiveItem: {itemName}, which shouldn't be used manually.");
        ApplyEffect(player);
    }
}

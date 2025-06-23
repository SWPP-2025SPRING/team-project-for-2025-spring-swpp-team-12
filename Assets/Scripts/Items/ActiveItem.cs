using UnityEngine;  

[CreateAssetMenu(fileName = "New Active Item", menuName = "Item/Active")]
public class ActiveItem : Item
{
    public override void OnPickup(GameObject player)
    {
        Debug.Log($"[Item] {itemName} picked up.");
        Inventory.Instance.AddActiveItem(this);
    }

    public override void OnUse(GameObject player)
    {
        Debug.Log($"[Item] {itemName} used.");
        ApplyEffect(player);  // 실제 효과 실행 (IEffect 사용 시)
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Item/Consumable")]
public class ConsumableItem : Item
{
    public override void OnPickup(GameObject player)
    {
        Debug.Log($"{itemName} consumed by {player.name}");
        ApplyEffect(player); // 즉시 효과 발동
    }

    public override void OnUse(GameObject player)
    {
        // ConsumableItem은 인벤토리에 들어가지 않기 때문에 이 함수는 비워도 됨
        Debug.Log($"[WARN] OnUse was called on a ConsumableItem: {itemName}, which should not be used manually.");
    }
}

using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Inventory.Instance.Add(itemData))
            {
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item itemData; 
    [SerializeField] private bool destroyOnPickup = true; // 아이템 획득 후 오브젝트 제거 여부
    [SerializeField] private GameObject pickupEffectPrefab; // 파티클 이펙트 프리팹

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTrigger");
        if (other.CompareTag("Player") && itemData != null)
        {
            Debug.Log($"[Item] {itemData.itemName} OnPickup called");
            itemData.OnPickup(other.gameObject);  // 아이템 효과 처리

            if (pickupEffectPrefab != null)
            {
                GameObject effect = Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 1.0f); // 일정 시간 후 이펙트 제거
            }

            if (destroyOnPickup)
            {
                Destroy(gameObject);  // 아이템 오브젝트 제거
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public GameObject slotPrefab;   // 프리팹 (슬롯용)
    public Transform slotParent;    // 슬롯이 들어갈 부모 (ex: GridLayoutGroup)

    private int maxSlots = 4;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

public void UpdateUI()
{
    Debug.Log("[UI] UpdateUI called");

    // 기존 슬롯 제거
    foreach (Transform child in slotParent)
    {
        Destroy(child.gameObject);
    }

    Debug.Log($"[UI] activeItems: {Inventory.Instance.activeItems.Count}, passiveItems: {Inventory.Instance.passiveItems.Count}");

    for (int i = 0; i < maxSlots; i++)
    {
        GameObject slot = Instantiate(slotPrefab, slotParent);
        Image itemImage = slot.transform.Find("ItemImage").GetComponent<Image>();

        if (i < Inventory.Instance.activeItems.Count)
        {
            itemImage.sprite = Inventory.Instance.activeItems[i].thumbnail;
            itemImage.color = new Color(1, 1, 1, 1);
            Debug.Log($"[UI] Slot {i} - ActiveItem: {Inventory.Instance.activeItems[i].itemName}");
        }
        else if (i < Inventory.Instance.activeItems.Count + Inventory.Instance.passiveItems.Count)
        {
            int idx = i - Inventory.Instance.activeItems.Count;
            itemImage.sprite = Inventory.Instance.passiveItems[idx].thumbnail;
            itemImage.color = new Color(1, 1, 1, 1);
            Debug.Log($"[UI] Slot {i} - PassiveItem: {Inventory.Instance.passiveItems[idx].itemName}");
        }
        else
        {
            itemImage.sprite = null;
            itemImage.color = new Color(1, 1, 1, 0);
            Debug.Log($"[UI] Slot {i} - Empty");
        }
    }
}
}

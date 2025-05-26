using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;         
    public Transform slotParent;          
    public static InventoryUI Instance;

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
        // 기존 슬롯 제거
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }

        int maxSlots = 4;

        for (int i = 0; i < maxSlots; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);

            Transform itemImageTr = slot.transform.Find("ItemImage");
            Image itemImage = itemImageTr.GetComponent<Image>();

            if (i < Inventory.Instance.items.Count)
            {
                itemImage.sprite = Inventory.Instance.items[i].thumbnail;
                itemImage.color = new Color(1, 1, 1, 1); // 보이게
            }
            else
            {
                itemImage.sprite = null;
                itemImage.color = new Color(1, 1, 1, 0); // 완전 투명
            }
        }
    }
}

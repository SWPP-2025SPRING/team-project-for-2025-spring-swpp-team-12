using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<ActiveItem> activeItems = new List<ActiveItem>();
    public List<PassiveItem> passiveItems = new List<PassiveItem>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddActiveItem(ActiveItem item)
    {
        activeItems.Add(item);
        Debug.Log($"[Inventory] ActiveItem added: {item.itemName}");
        Debug.Log($"[Inventory] ActiveItem Count: {activeItems.Count}");
        InventoryUI.Instance?.UpdateUI();  // UI 갱신
    }

    public void AddPassiveItem(PassiveItem item)
    {
        passiveItems.Add(item);
        Debug.Log($"[Inventory] PassiveItem added: {item.itemName}");
        Debug.Log($"[Inventory] PassiveItem Count: {passiveItems.Count}");
        InventoryUI.Instance?.UpdateUI();  // UI 갱신

        StartCoroutine(RemoveAfterDelay(item, item.duration));
    }


    private IEnumerator RemoveAfterDelay(PassiveItem item, float delay)
    {
        item.OnUse(GameObject.FindGameObjectWithTag("Player"));
        yield return new WaitForSeconds(delay);
        passiveItems.Remove(item);
        Debug.Log("PassiveItem removed: " + item.itemName);
        InventoryUI.Instance?.UpdateUI();  // 제거 시도 후 UI 갱신
    }

    public void UseActiveItem(int index)
    {
        if (index < 0 || index >= activeItems.Count) return;

        var item = activeItems[index];

        // 🔥 실제 효과 적용
        item.OnUse(GameObject.FindGameObjectWithTag("Player"));  // <== 여기서 호출!

        Debug.Log("ActiveItem used: " + item.itemName);
        activeItems.RemoveAt(index);
        InventoryUI.Instance?.UpdateUI();
    }
}

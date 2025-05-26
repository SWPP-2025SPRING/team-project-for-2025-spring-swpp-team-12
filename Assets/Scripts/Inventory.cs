using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<Item> items = new List<Item>();
    public int maxSlots = 20;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool Add(Item item)
    {
        if (items.Count >= maxSlots) return false;

        items.Add(item);


        if (InventoryUI.Instance != null)
            InventoryUI.Instance.UpdateUI();

        return true;
    }
}

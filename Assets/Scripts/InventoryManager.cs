using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public int maxStackedItems = 10;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    int selectedSloth = -1;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        ChangeSelectedColor(0);
    }
    void Update()
    {
        if(Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number > 0 && number < 8)
            {
                ChangeSelectedColor(number - 1);
                Debug.Log(number);
            }
        }
    }
    void ChangeSelectedColor(int newValue)
    {
        if(selectedSloth >= 0)
        {
            inventorySlots[selectedSloth].Deselect();

        }
        inventorySlots[newValue].Select();
        selectedSloth = newValue;
    }

    public bool AddItem(Item item)
{
    // Primero buscar stack existente
    foreach (var slot in inventorySlots)
    {
        InventoryItem itemEnSlot = slot.itemActual;
        if (itemEnSlot != null && itemEnSlot.item == item 
            && itemEnSlot.count < maxStackedItems)
        {
            itemEnSlot.count++;
            itemEnSlot.RefreshCount();
            return true;
        }
    }

    // Luego slot vacío
    foreach (var slot in inventorySlots)
    {
        if (slot.itemActual == null)
        {
            SpawnNewItem(item, slot);
            return true;
        }
    }

    return false;
}

void SpawnNewItem(Item item, InventorySlot slot)
{
    GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
    InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
    inventoryItem.InitialiseItem(item);
    slot.SetItem(inventoryItem); // registrar en el slot
}

public Item GetSelectedItem(bool use)
{
    InventorySlot slot = inventorySlots[selectedSloth];
    InventoryItem itemEnSlot = slot.itemActual;

    if (itemEnSlot == null) return null;

    Item item = itemEnSlot.item;

    if (use)
    {
        itemEnSlot.count--;
        if (itemEnSlot.count <= 0)
        {
            slot.ClearItem();
            Destroy(itemEnSlot.gameObject);
        }
        else
        {
            itemEnSlot.RefreshCount();
        }
    }

    return item;
}
}

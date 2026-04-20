// InventorySlot.cs

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedCol, unselectedCol;

    void Awake()
    {
        Debug.Log("Slot activo: " + gameObject.name + " | Image: " + image);
        Deselect();
    }
    public void Select()
    {
        image.color = selectedCol;
    }
    public void Deselect()
    {
        image.color = unselectedCol;
    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop llamado en: " + gameObject.name);
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();

        if (inventoryItem == null) return;

        if (transform.childCount == 0)
        {
            inventoryItem.parentAfterDrag = transform; 
        }
    }
}
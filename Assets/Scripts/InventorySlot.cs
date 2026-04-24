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
        InventoryItem itemArrastrado = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (itemArrastrado == null) return;

        InventoryItem itemEnSlot = GetComponentInChildren<InventoryItem>();

        if (itemEnSlot == null)
        {
        // Slot vacío — simplemente mover
            itemArrastrado.parentAfterDrag = transform;
        }
        else if (itemEnSlot != itemArrastrado)
        {
        // Slot ocupado — intercambiar posiciones
            Transform slotOrigen = itemArrastrado.parentAfterDrag;

            itemArrastrado.parentAfterDrag = transform;
            itemEnSlot.transform.SetParent(slotOrigen);
            itemEnSlot.transform.localPosition = Vector3.zero;
        }
    }
    public InventoryItem itemActual { get; private set; }

    public void SetItem(InventoryItem item)
    {
        itemActual = item;
    }

    public void ClearItem()
    {
        itemActual = null;
    }
}
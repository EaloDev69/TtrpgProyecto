// TestInventory.cs — ponlo en cualquier GameObject de la escena
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    [Header("Item de prueba (arrastra un asset Item aqui)")]
    public Item itemDePrueba;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (itemDePrueba == null)
            {
                Debug.LogWarning("TestInventory: no hay item asignado.");
                return;
            }

            bool agregado = InventoryManager.instance.AddItem(itemDePrueba);
            Debug.Log(agregado ? $"Item agregado: {itemDePrueba.name}" : "Inventario lleno.");
        }
    }
}
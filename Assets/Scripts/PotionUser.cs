// PotionUser.cs — ponlo en el mismo GameObject que BattleManager
using UnityEngine;

public class PotionUser : MonoBehaviour
{
    [Header("Configuracion")]
    public KeyCode teclaPociion  = KeyCode.H;
    public float   cantidadCura  = 30f;
    public float   saludMaxima   = 100f;

    void Update()
    {
        if (!Input.GetKeyDown(teclaPociion))       return;
        if (BattleManager.Instance == null)         return;
        if (!BattleManager.Instance.PlayerTurn)     return;
        if (BattleManager.Instance.EncuentroTerminado) return;

        UsarPocion();
    }

    void UsarPocion()
    {
        // 1. Obtener item seleccionado sin consumirlo aún
        Item item = InventoryManager.instance.GetSelectedItem(false);

        if (item == null)
        {
            Debug.Log("No hay item seleccionado.");
            return;
        }

        // 2. Verificar que sea una poción de cura
        if (item.type != Item.ItemType.Consumable || item.actionType != Item.ActionType.Heal)
        {
            Debug.Log("El item seleccionado no es una poción de cura.");
            return;
        }

        // 3. Obtener el miembro actual
        PartyMember miembro = BattleManager.Instance.ObtenerMiembroActual();

        if (miembro == null || !miembro.vivo)
        {
            Debug.Log("No hay miembro activo para curar.");
            return;
        }

        // 4. Verificar que no esté al máximo (no gastar la poción en vano)
        if (miembro.salud >= saludMaxima)
        {
            Debug.Log($"{miembro.nombrePersonaje} ya tiene la salud al máximo.");
            return;
        }

        // 5. Consumir el item del inventario
        InventoryManager.instance.GetSelectedItem(true);

        // 6. Curar respetando el máximo
        float saludAntes = miembro.salud;
        miembro.salud = Mathf.Min(miembro.salud + cantidadCura, saludMaxima);
        float curado  = miembro.salud - saludAntes;

        Debug.Log($"{miembro.nombrePersonaje} curado en {curado} HP. " +
                  $"Salud: {miembro.salud}/{saludMaxima}");

        // 7. Marcar como actuado y avanzar turno (consume el turno como atacar)
        miembro.yaActuo = true;
        BattleManager.Instance.AvanzarMiembroActual();
    }
}

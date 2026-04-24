using UnityEngine;

/// <summary>
/// Adjunta este script a los botones de ataque de tu UI.
/// Cada botón llama a BotonAtacar(indice) con el índice del ataque (0, 1 o 2).
/// </summary>
public class Ataque : MonoBehaviour
{
    // ─── Botones de ataque ────────────────────────────────────────────────────

    /// <summary>
    /// Llama este método desde el botón de UI con el índice del ataque.
    /// 0 = primer ataque, 1 = segundo, 2 = tercero.
    /// Ejemplo en el Inspector del Button: BotonAtacar(0)
    /// </summary>
    public void BotonAtacar(int indiceAtaque)
    {
        if (!ValidarEstadoCombate()) return;

        PartyMember atacante = BattleManager.Instance.ObtenerMiembroActual();
        if (!ValidarAtacante(atacante))  return;

        // Comprueba que el personaje tenga ese índice de ataque
        if (atacante.datos == null || indiceAtaque >= atacante.datos.ataques.Length)
        {
            Debug.Log($"Ataque: {atacante.nombrePersonaje} no tiene un ataque en el índice {indiceAtaque}.");
            return;
        }

        Enemigo objetivo = BattleManager.Instance.enemigoSeleccionado;
        if (!ValidarObjetivo(objetivo)) return;

        // EjecutarAtaque devuelve true si conectó, false si fue pifia
        bool conecto = atacante.EjecutarAtaque(indiceAtaque, objetivo);
        string resultado = conecto ? "conectó" : "falló (pifia)";
        Debug.Log($"[{atacante.nombrePersonaje}] {atacante.datos.ataques[indiceAtaque].nombreAtaque} {resultado}.");

        TerminarAccion(atacante);
    }

    // ─── Botón siguiente ──────────────────────────────────────────────────────

    public void BotonSiguiente()
    {
        if (BattleManager.Instance == null)            return;
        if (!BattleManager.Instance.PlayerTurn)        { Debug.Log("Siguiente: no es turno del jugador."); return; }
        if (BattleManager.Instance.EncuentroTerminado) return;

        PartyMember actual = BattleManager.Instance.ObtenerMiembroActual();
        if (actual == null || !actual.yaActuo)
        {
            Debug.Log("Siguiente: el miembro actual aún no ha actuado.");
            return;
        }

        BattleManager.Instance.AvanzarMiembroActual();
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────

    private void TerminarAccion(PartyMember atacante)
    {
        atacante.yaActuo = true;
        Debug.Log($"[{atacante.nombrePersonaje}] acción terminada. Pulsa Siguiente para continuar.");
    }

    private bool ValidarEstadoCombate()
    {
        if (BattleManager.Instance == null)
        {
            Debug.LogError("Ataque: no se encontró GameManager.");
            return false;
        }
        if (!BattleManager.Instance.PlayerTurn)
        {
            Debug.Log("Ataque: no es el turno del jugador.");
            return false;
        }
        if (BattleManager.Instance.EncuentroTerminado)
        {
            Debug.Log("Ataque: el encuentro ya terminó.");
            return false;
        }
        return true;
    }

    private bool ValidarAtacante(PartyMember atacante)
    {
        if (atacante == null || !atacante.vivo)
        {
            Debug.Log("Ataque: no hay atacante válido.");
            return false;
        }
        if (atacante.yaActuo)
        {
            Debug.Log($"{atacante.nombrePersonaje} ya actuó este turno.");
            return false;
        }
        return true;
    }

    private bool ValidarObjetivo(Enemigo objetivo)
    {
        if (objetivo == null)
        {
            Debug.Log("Ataque: no hay enemigo seleccionado.");
            return false;
        }
        if (!objetivo.vivo)
        {
            Debug.Log("Ataque: ese enemigo ya está muerto.");
            return false;
        }
        return true;
    }
}

using UnityEngine;

/// <summary>
/// Adjunta este script a los botones de ataque de tu UI en la Boss Battle.
/// Cada botón llama a BotonAtacar(indice) con el índice del ataque (0, 1 o 2).
/// </summary>
public class AtaqueBossBattle : MonoBehaviour
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

        PartyMemberBoss atacante = BattleManagerBoss.Instance.ObtenerMiembroActual();
        if (!ValidarAtacante(atacante)) return;

        // Comprueba que el personaje tenga ese índice de ataque
        if (atacante.datos == null || indiceAtaque >= atacante.datos.ataques.Length)
        {
            Debug.Log($"Ataque: {atacante.nombrePersonaje} no tiene un ataque en el índice {indiceAtaque}.");
            return;
        }

        EnemigoIA objetivo = BattleManagerBoss.Instance.enemigoSeleccionado;
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
        if (BattleManagerBoss.Instance == null)            return;
        if (!BattleManagerBoss.Instance.PlayerTurn)        { Debug.Log("Siguiente: no es turno del jugador."); return; }
        if (BattleManagerBoss.Instance.EncuentroTerminado) return;

        PartyMemberBoss actual = BattleManagerBoss.Instance.ObtenerMiembroActual();
        if (actual == null || !actual.yaActuo)
        {
            Debug.Log("Siguiente: el miembro actual aún no ha actuado.");
            return;
        }

        BattleManagerBoss.Instance.AvanzarMiembroActual();
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────

    private void TerminarAccion(PartyMemberBoss atacante)
    {
        atacante.yaActuo = true;
        Debug.Log($"[{atacante.nombrePersonaje}] acción terminada. Pulsa Siguiente para continuar.");
    }

    private bool ValidarEstadoCombate()
    {
        if (BattleManagerBoss.Instance == null)
        {
            Debug.LogError("AtaqueBossBattle: no se encontró BattleManagerBoss.");
            return false;
        }
        if (!BattleManagerBoss.Instance.PlayerTurn)
        {
            Debug.Log("AtaqueBossBattle: no es el turno del jugador.");
            return false;
        }
        if (BattleManagerBoss.Instance.EncuentroTerminado)
        {
            Debug.Log("AtaqueBossBattle: el encuentro ya terminó.");
            return false;
        }
        return true;
    }

    private bool ValidarAtacante(PartyMemberBoss atacante)
    {
        if (atacante == null || !atacante.vivo)
        {
            Debug.Log("AtaqueBossBattle: no hay atacante válido.");
            return false;
        }
        if (atacante.yaActuo)
        {
            Debug.Log($"{atacante.nombrePersonaje} ya actuó este turno.");
            return false;
        }
        return true;
    }

    private bool ValidarObjetivo(EnemigoIA objetivo)
    {
        if (objetivo == null)
        {
            Debug.Log("AtaqueBossBattle: no hay enemigo seleccionado.");
            return false;
        }
        if (!objetivo.vivo)
        {
            Debug.Log("AtaqueBossBattle: ese enemigo ya está muerto.");
            return false;
        }
        return true;
    }
}

using UnityEngine;

public class Ataque : MonoBehaviour
{
    public void BotonAtacar()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("Ataque: no se encontró GameManager.");
            return;
        }
        if (!GameManager.Instance.PlayerTurn)        { Debug.Log("Ataque: no es el turno del jugador."); return; }
        if (GameManager.Instance.EncuentroTerminado) { Debug.Log("Ataque: el encuentro ya terminó.");    return; }

        PartyMember atacante = GameManager.Instance.ObtenerMiembroActual();
        if (atacante == null || !atacante.vivo) { Debug.Log("Ataque: no hay atacante válido."); return; }
        if (atacante.yaActuo)                   { Debug.Log(atacante.nombrePersonaje + " ya actuó este turno."); return; }

        Enemigo objetivo = GameManager.Instance.enemigoSeleccionado;
        if (objetivo == null)  { Debug.Log("Ataque: no hay enemigo seleccionado."); return; }
        if (!objetivo.vivo)    { Debug.Log("Ataque: ese enemigo ya está muerto."); return; }

        PruebaDado(atacante, objetivo);
    }

    public void BotonSiguiente()
    {
        if (GameManager.Instance == null)            return;
        if (!GameManager.Instance.PlayerTurn)        { Debug.Log("Siguiente: no es turno del jugador."); return; }
        if (GameManager.Instance.EncuentroTerminado) return;

        PartyMember actual = GameManager.Instance.ObtenerMiembroActual();
        if (actual == null || !actual.yaActuo)       { Debug.Log("Siguiente: el miembro actual aún no ha actuado."); return; }

        GameManager.Instance.AvanzarMiembroActual();
    }

    private void PruebaDado(PartyMember atacante, Enemigo objetivo)
    {
        float exito = Random.Range(0, 10);
        Debug.Log("[" + atacante.nombrePersonaje + "] Tirada de prueba: " + exito);

        if (exito <= 3) Pifia(atacante);
        else            Daño(atacante, objetivo);
    }

    private void Pifia(PartyMember atacante)
    {
        Debug.Log("[" + atacante.nombrePersonaje + "] Pifia.");
        TerminarAccion(atacante);
    }

    private void Daño(PartyMember atacante, Enemigo objetivo)
    {
        float decenas       = Random.Range(0, 10) * 10;
        float unidades      = Random.Range(0, 10);
        float segundaPrueba = decenas + unidades;
        Debug.Log("[" + atacante.nombrePersonaje + "] Tirada percentil: " + segundaPrueba);

        if (segundaPrueba >= 90 || segundaPrueba < 50)
        {
            Debug.Log("[" + atacante.nombrePersonaje + "] Fallo en segunda prueba.");
            Pifia(atacante);
            return;
        }

        float dañoFinal = Random.Range(0, 11);
        Debug.Log("[" + atacante.nombrePersonaje + "] ataca a " +
                  objetivo.nombreEnemigo + ". Daño: " + dañoFinal);
        objetivo.RecibirDaño(dañoFinal);
        TerminarAccion(atacante);
    }

    private void TerminarAccion(PartyMember atacante)
    {
        atacante.yaActuo = true;
        Debug.Log("[" + atacante.nombrePersonaje + "] acción terminada. Pulsa Siguiente para continuar.");
    }
}
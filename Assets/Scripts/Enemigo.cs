using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public string nombreEnemigo = "Enemigo";
    public float  salud         = 100f;
    public bool   vivo          = true;
    public bool   yaActuo       = false;
    
    public void RecibirDaño(float cantidad)
    {
        if (!vivo) return;

        salud = Mathf.Max(0f, salud - cantidad);
        Debug.Log(nombreEnemigo + " recibió " + cantidad + " de daño. Salud: " + salud);

        if (salud <= 0f)
        {
            vivo    = false;
            yaActuo = true;
            Debug.Log(nombreEnemigo + " ha muerto.");

            // Si este era el enemigo seleccionado, GameManager auto-selecciona el siguiente
            if (BattleManager.Instance != null)
                BattleManager.Instance.ComprobarVictoria();
        }
    }
    

    public void EjecutarTurno()
    {
        float exito = Random.Range(0, 10);
        Debug.Log("[" + nombreEnemigo + "] Tirada de prueba: " + exito);

        if (exito <= 3)
            PifiaIA();
        else
            DadoDañoIA();
    }

    private void PifiaIA()
    {
        Debug.Log("[" + nombreEnemigo + "] Pifia. Cede el turno.");
        yaActuo = true;
    }

    private void DadoDañoIA()
    {
        float decenas       = Random.Range(0, 10) * 10;
        float unidades      = Random.Range(0, 10);
        float segundaPrueba = decenas + unidades;

        Debug.Log("[" + nombreEnemigo + "] Tirada percentil: " + segundaPrueba);

        if (segundaPrueba >= 90 || segundaPrueba < 50)
        {
            PifiaIA();
            return;
        }

        float dañoFinal = Random.Range(0, 11);

        PartyMember objetivo = BattleManager.Instance != null
            ? BattleManager.Instance.ObtenerMiembroVivoAleatorio()
            : null;

        if (objetivo != null)
        {
            Debug.Log("[" + nombreEnemigo + "] Ataca a " +
                      objetivo.nombrePersonaje + ". Daño: " + dañoFinal);
            objetivo.RecibirDaño(dañoFinal);
        }

        yaActuo = true;
    }
}
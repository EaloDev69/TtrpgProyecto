using UnityEngine;


public class EnemigoIA : MonoBehaviour
{
    [Header("Stats")]
    public string nombreEnemigo = "Enemigo";
    public float  salud         = 100f;
    public bool   vivo          = true;
    public bool   yaActuo       = false;
    
 
    public void RecibirDaño(float cantidad)
    {
        if (!vivo) return;
 
        salud = Mathf.Max(0f, salud - cantidad);
        Debug.Log(nombreEnemigo + " recibió " + cantidad + " de daño. Salud restante: " + salud);
 
        if (salud <= 0f)
        {
            vivo    = false;
            yaActuo = true;
            Debug.Log(nombreEnemigo + " ha muerto.");
            gameObject.SetActive(false);
 
            if (BattleManagerBoss.Instance != null)
                BattleManagerBoss.Instance.ComprobarVictoria();
        }
    }
    
    
    public virtual void EjecutarTurno()
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
 
        PartyMemberBoss objetivo = BattleManagerBoss.Instance != null
            ? BattleManagerBoss.Instance.ObtenerMiembroVivoAleatorio()
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

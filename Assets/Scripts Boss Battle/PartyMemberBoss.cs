using UnityEngine;

public class PartyMemberBoss : MonoBehaviour
{
    [Header("Datos del personaje (arrastra el .asset aquí)")]
    public PersonajeSO datos;
 
    // Estado en combate (no va en el SO porque cambia en runtime)
    [HideInInspector] public float  salud;
    [HideInInspector] public bool   vivo    = true;
    [HideInInspector] public bool   yaActuo = false;
 
    // Acceso rápido al nombre sin tener que escribir datos.nombrePersonaje
    public string nombrePersonaje => datos != null ? datos.nombrePersonaje : "Sin datos";
    
 
    void Awake()
    {
        if (datos == null)
        {
            Debug.LogError($"[PartyMemberBoss] {name} no tiene un PersonajeSO asignado.");
            return;
        }
        salud = datos.saludMaxima;
    }
    
    
    public bool EjecutarAtaque(int indiceAtaque, EnemigoIA objetivo)
    {
        if (datos == null)                          return false;
        if (indiceAtaque >= datos.ataques.Length)   return false;
        if (objetivo == null || !objetivo.vivo)     return false;
 
        AtaqueDato atq = datos.ataques[indiceAtaque];
 
        // Primera prueba (d10): pifia con 0–3
        int prueba1 = Random.Range(0, 10);
        Debug.Log($"[{nombrePersonaje}] {atq.nombreAtaque} – Prueba 1: {prueba1}");
 
        if (prueba1 <= 3)
        {
            Debug.Log($"[{nombrePersonaje}] Pifia en {atq.nombreAtaque}.");
            return false;   // Pifia: no hay daño, el turno se marca como actuado igual
        }
 
        // Segunda prueba percentil: éxito en 50–89
        int decenas  = Random.Range(0, 10) * 10;
        int unidades = Random.Range(0, 10);
        int prueba2  = decenas + unidades;
        Debug.Log($"[{nombrePersonaje}] {atq.nombreAtaque} – Prueba percentil: {prueba2}");
 
        if (prueba2 < 50 || prueba2 >= 90)
        {
            Debug.Log($"[{nombrePersonaje}] Fallo en segunda prueba de {atq.nombreAtaque}.");
            return false;
        }
 
        // Daño dentro del rango definido en el SO
        int dañoFinal = Random.Range(atq.dañoMin, atq.dañoMax + 1);
        Debug.Log($"[{nombrePersonaje}] {atq.nombreAtaque} → {objetivo.nombreEnemigo}. Daño: {dañoFinal}");
        objetivo.RecibirDaño(dañoFinal);
 
        return true;
    }
 
    public void RecibirDaño(float cantidad)
    {
        if (!vivo) return;
 
        salud = Mathf.Max(0f, salud - cantidad);
        Debug.Log($"{nombrePersonaje} recibió {cantidad} de daño. Salud: {salud}");
 
        if (salud <= 0f)
        {
            vivo    = false;
            yaActuo = true;
            Debug.Log($"{nombrePersonaje} ha muerto.");
            gameObject.SetActive(false);
 
            if (BattleManagerBoss.Instance != null)
            {
                BattleManagerBoss.Instance.ComprobarDerrota();
                if (!BattleManagerBoss.Instance.EncuentroTerminado)
                    BattleManagerBoss.Instance.ComprobarFinTurnoParty();
            }
        }
    }
}
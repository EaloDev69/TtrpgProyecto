using UnityEngine;

public class PartyMember : MonoBehaviour
{
    [Header("Datos del personaje (arrastra el .asset aquí)")]
    public PersonajeSO datos;

    // Estado en combate (no va en el SO porque cambia en runtime)
    [HideInInspector] public float  salud = 100f;
    [HideInInspector] public bool   vivo    = true;
    [HideInInspector] public bool   yaActuo = false;

    // Acceso rápido al nombre sin tener que escribir datos.nombrePersonaje
    public string nombrePersonaje => datos != null ? datos.nombrePersonaje : "Sin datos";
    public float saludMaxima => datos != null ? datos.saludMaxima : 100f;

    // ─── Inicialización ───────────────────────────────────────────────────────

    void Awake()
    {
        if (datos == null)
        {
            Debug.LogError($"[PartyMember] {name} no tiene un PersonajeSO asignado.");
            return;
        }
        salud = datos.saludMaxima;
    }

    // ─── Ataques ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Ejecuta el ataque en el índice indicado (0, 1 o 2) contra el objetivo.
    /// Devuelve true si el ataque conectó (para que Ataque.cs decida si llama a TerminarAccion).
    /// </summary>
    public bool EjecutarAtaque(int indiceAtaque, Enemigo objetivo)
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
        int decenas       = Random.Range(0, 10) * 10;
        int unidades      = Random.Range(0, 10);
        int prueba2       = decenas + unidades;
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

    // ─── Daño recibido ────────────────────────────────────────────────────────

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

            if (BattleManager.Instance != null)
            {
                BattleManager.Instance.ComprobarDerrota();
                if (!BattleManager.Instance.EncuentroTerminado)
                    BattleManager.Instance.ComprobarFinTurnoParty();
            }
        }
    }
}

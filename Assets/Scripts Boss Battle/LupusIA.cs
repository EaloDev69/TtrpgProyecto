using UnityEngine;

public class LupusIA : EnemigoIA
{
    [Header("Referencia a Helena")]
    [SerializeField] private HelenaIA helena;
    
    void Start()
    {
        nombreEnemigo = "Lupus";
        salud         = 83f;

        if (helena == null)
            Debug.LogWarning("[Lupus] No se asignó Helena en el Inspector.");
    }
    
    public override void EjecutarTurno()
    {
        if (!vivo) return;

        Debug.Log("=== TURNO DE LUPUS ===");

        bool helenaViva = helena != null && helena.vivo;

        // Tirada de éxito 1D10
        int tirada = Random.Range(0, 10); // 0–9
        Debug.Log($"[Lupus] Tirada de éxito: {tirada}");

        if (helenaViva)
            TurnoConHelena(tirada);
        else
            TurnoSinHelena(tirada);

        yaActuo = true;
    }
    
    private void TurnoConHelena(int tirada)
    {
        // Pifia: 8 o 9 → Helena no ataca
        if (tirada > 7 && tirada <= 9)
        {
            Debug.Log("[Lupus] Pifia. Helena no ataca este turno.");
            AtacarLupus(); // Lupus igual ataca por su cuenta
            return;
        }

        // Éxito: 1–6 → Helena ataca junto a Lupus
        if (tirada > 0 && tirada < 7)
        {
            Debug.Log("[Lupus] Éxito. Helena ataca en conjunto.");
            AtacarLupus();
            helena.AtaqueDeApoyo();
            return;
        }

        // Tirada 0 → pifia total
        Debug.Log("[Lupus] Tirada 0. Pifia total, nadie ataca.");
    }


    private void TurnoSinHelena(int tirada)
    {
        // Pifia: 8 o 9 → Lupus no ataca
        if (tirada > 7 && tirada <= 9)
        {
            Debug.Log("[Lupus] Sin Helena. Pifia, Lupus no ataca.");
            return;
        }

        // Éxito: 1–6 → Lupus ataca
        if (tirada > 0 && tirada < 7)
        {
            Debug.Log("[Lupus] Sin Helena. Éxito, Lupus ataca.");
            AtacarLupus();
            return;
        }

        // Tirada 0 → pifia total
        Debug.Log("[Lupus] Tirada 0. Pifia total.");
    }
    

    private void AtacarLupus()
    {
        PartyMemberBoss objetivo = BattleManagerBoss.Instance != null
            ? BattleManagerBoss.Instance.ObtenerMiembroVivoAleatorio()
            : null;

        if (objetivo == null)
        {
            Debug.Log("[Lupus] No hay objetivos vivos.");
            return;
        }

        int daño = Random.Range(0, 6) + Random.Range(0, 8); // 0-5 + 0-7
        Debug.Log($"[Lupus] Ataca a {objetivo.nombrePersonaje}. Daño: {daño}");
        objetivo.RecibirDaño(daño);
    }
}

using UnityEngine;


public class HelenaIA : EnemigoIA
{

    void Start()
    {
        nombreEnemigo = "Helena";
        salud         = 34f;
    }
    
    public override void EjecutarTurno()
    {
        // Helena no actúa por su cuenta — no hacer nada aquí.
        yaActuo = true;
    }
    
    public void AtaqueDeApoyo()
    {
        if (!vivo)
        {
            Debug.Log("[Helena] Está muerta, no puede atacar.");
            return;
        }

        PartyMemberBoss objetivo = BattleManagerBoss.Instance != null
            ? BattleManagerBoss.Instance.ObtenerMiembroVivoAleatorio()
            : null;

        if (objetivo == null)
        {
            Debug.Log("[Helena] No hay objetivos vivos.");
            return;
        }

        int daño = Random.Range(0, 10) + Random.Range(0, 8); // 0-9 + 0-7
        Debug.Log($"[Helena] Ataque de apoyo a {objetivo.nombrePersonaje}. Daño: {daño}");
        objetivo.RecibirDaño(daño);
    }
}

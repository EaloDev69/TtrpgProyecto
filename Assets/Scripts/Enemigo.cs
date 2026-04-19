using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public static Enemigo Instance { get; private set; }

    private float Salud = 100f;

    public float ExitoIA;
    public float DecenasIA;
    public float UnidadesIA;
    public float SegundaPruebaIA;
    public float DañoFinalIA;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update() { }

    public void RecibirDaño(float cantidad)
    {
        Salud -= cantidad;
        Debug.Log("Enemigo recibió " + cantidad + " de daño. Salud restante: " + Salud);

        if (Salud <= 0)
        {
            Salud = 0;
            if (GameManager.Instance != null)
                GameManager.Instance.EncuentroGanado();
        }
        else
        {
            if (GameManager.Instance != null)
                GameManager.Instance.IniciarTurnoJugador();
        }
    }

    public void DañoJugador()
    {
        ExitoIA = Random.Range(0, 9);
        Debug.Log("[Enemigo] Tirada de prueba: " + ExitoIA);

        if (ExitoIA <= 3)
            PifiaIA();
        else
            DadoDañoIA();
    }

    private void PifiaIA()
    {
        Debug.Log("[Enemigo] Pifia. Cede el turno al jugador.");
        if (GameManager.Instance != null)
            GameManager.Instance.IniciarTurnoJugador();
    }

    private void DadoDañoIA()
    {
        DecenasIA       = Random.Range(0, 9) * 10;
        UnidadesIA      = Random.Range(0, 9);
        SegundaPruebaIA = DecenasIA + UnidadesIA;
        Debug.Log("[Enemigo] Segunda prueba de daño: " + SegundaPruebaIA);

        if (SegundaPruebaIA >= 90)
        {
            Debug.Log("[Enemigo] Pifia crítica. Cede el turno al jugador.");
            PifiaIA();
        }
        else if (SegundaPruebaIA < 50)
        {
            Debug.Log("[Enemigo] Fallo en segunda prueba.");
            PifiaIA();
        }
        else
        {
            DañoFinalIA = Random.Range(0, 11);
            Debug.Log("[Enemigo] Ataque exitoso. Daño causado: " + DañoFinalIA);

            if (PlayerManager.Instance != null)
                PlayerManager.Instance.RecibirDaño(DañoFinalIA);

            if (GameManager.Instance != null && !GameManager.Instance.EncuentroTerminado)
                GameManager.Instance.IniciarTurnoJugador();
        }
    }
}
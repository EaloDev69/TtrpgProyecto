using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public static Enemigo Instance { get; private set; }

    private float Salud  = 100f;
    public  float DadoAC = 0f;
    public  float Daño   = 0f;

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

    void Update()
    {
        if (Salud <= 0)
        {
            Debug.Log("El enemigo ha muerto.");
            Destroy(gameObject);
        }
    }

    public void RecibirDaño(float cantidad)
    {
        Salud -= cantidad;
        Debug.Log("Enemigo recibió " + cantidad + " de daño. Salud restante: " + Salud);
        if (GameManager.Instance != null)
            GameManager.Instance.IniciarTurnoEnemigo();
    }

    public void DañoJugador()
    {
        DadoAC = Random.Range(0, 20);

        if (DadoAC >= 14)
        {
            Debug.Log("Enemigo acierta. Dado: " + DadoAC);
            DadoDaño();
        }
        else
        {
            Debug.Log("Enemigo falló su ataque. Dado: " + DadoAC);
            if (GameManager.Instance != null)
                GameManager.Instance.IniciarTurnoJugador();
        }
    }

    private void DadoDaño()
    {
        Daño = Random.Range(1, 11);
        Debug.Log("Enemigo hace " + Daño + " de daño al jugador.");
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.RecibirDaño(Daño);
        if (GameManager.Instance != null)
            GameManager.Instance.IniciarTurnoJugador();
    }
}
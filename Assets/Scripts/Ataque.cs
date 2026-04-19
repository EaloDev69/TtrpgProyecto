using UnityEngine;

public class Ataque : MonoBehaviour
{
    public static Ataque Instance { get; private set; }

    public float Exito;
    public float Unidades;
    public float Decenas;
    public float SegundaPrueba;
    public float DañoFinal;
    public bool  esperandoTirada = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void BotonAtacar()
    {
        if (GameManager.Instance == null)
        {
            Debug.Log("ERROR: GameManager.Instance es null");
            return;
        }

        if (GameManager.Instance.PlayerTurn && !esperandoTirada)
        {
            esperandoTirada = true;
            PruebaDado();
        }
        else
        {
            Debug.Log("No es tu turno o ya tiraste el dado.");
        }
    }

    private void PruebaDado()
    {
        Exito = Random.Range(0, 9);
        Debug.Log("Tirada de prueba: " + Exito);

        if (Exito <= 3)
            Pifia();
        else
            Daño();
    }

    private void Pifia()
    {
        Debug.Log("Pifia. Pierdes el turno.");

        if (GameManager.Instance != null)
            GameManager.Instance.IniciarTurnoEnemigo();
    }

    private void Daño()
    {
        Decenas      = Random.Range(0, 9) * 10;
        Unidades     = Random.Range(0, 9);
        SegundaPrueba = Decenas + Unidades;
        Debug.Log("Segunda prueba de daño: " + SegundaPrueba);

        if (SegundaPrueba >= 90)
        {
            Debug.Log("Pifia crítica. Pierdes el turno.");
            Pifia();
        }
        else if (SegundaPrueba < 50)
        {
            Debug.Log("Fallo en segunda prueba.");
            Pifia();
        }
        else
        {
            DañoFinal = Random.Range(0, 11);
            Debug.Log("Ataque exitoso. Daño causado: " + DañoFinal);

            if (Enemigo.Instance != null)
                Enemigo.Instance.RecibirDaño(DañoFinal);
        }
    }
}
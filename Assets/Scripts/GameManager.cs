using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float IniciativaPlayerIA;
    public float IniciativaPlayer;

    public bool PlayerTurn    = false;
    public bool PlayerIATurn  = false;

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
            return;
        }

        IniciativaPlayerIA = Random.Range(0, 10);
        IniciativaPlayer   = Random.Range(0, 10);
    }

    void Start()
    {
        if (IniciativaPlayerIA > IniciativaPlayer)
        {
            PlayerIATurn = true;
            PlayerTurn   = false;
            Debug.Log("Le toca al enemigo. IA: " + IniciativaPlayerIA + " | Jugador: " + IniciativaPlayer);
            if (Enemigo.Instance != null)
                Enemigo.Instance.DañoJugador();
        }
        else
        {
            PlayerTurn   = true;
            PlayerIATurn = false;
            Debug.Log("Le toca al jugador. Jugador: " + IniciativaPlayer + " | IA: " + IniciativaPlayerIA);
        }
    }

    void Update() { }

    public void IniciarTurnoJugador()
    {
        PlayerTurn   = true;
        PlayerIATurn = false;
        if (Ataque.Instance != null)
            Ataque.Instance.esperandoTirada = false;
        Debug.Log("=== TURNO DEL JUGADOR ===");
    }

    public void IniciarTurnoEnemigo()
    {
        PlayerTurn   = false;
        PlayerIATurn = true;
        Debug.Log("=== TURNO DEL ENEMIGO ===");
        if (Enemigo.Instance != null)
            Enemigo.Instance.DañoJugador();
    }
}

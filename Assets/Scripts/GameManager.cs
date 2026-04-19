using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float IniciativaPlayerIA;
    public float IniciativaPlayer;

    public bool PlayerTurn   = false;
    public bool PlayerIATurn = false;
    public bool EncuentroTerminado = false;

    [Header("Configuracion")]
    public string escenaSiguiente = "SiguienteEscena"; // Cambia por el nombre real de tu escena

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
        if (EncuentroTerminado) return;

        PlayerTurn   = true;
        PlayerIATurn = false;
        if (Ataque.Instance != null)
            Ataque.Instance.esperandoTirada = false;
        Debug.Log("=== TURNO DEL JUGADOR ===");
    }

    public void IniciarTurnoEnemigo()
    {
        if (EncuentroTerminado) return;

        PlayerTurn   = false;
        PlayerIATurn = true;
        Debug.Log("=== TURNO DEL ENEMIGO ===");
        if (Enemigo.Instance != null)
            Enemigo.Instance.DañoJugador();
    }

    public void EncuentroGanado()
    {
        if (EncuentroTerminado) return;
        EncuentroTerminado = true;

        PlayerTurn   = false;
        PlayerIATurn = false;

        Debug.Log("=== VICTORIA: El jugador ha ganado el encuentro ===");
        SceneManager.LoadScene(escenaSiguiente);
        //escenaSiguiente es un placeholder, cámbialo por el nombre real de tu escena de victoria o siguiente nivel
    }

    public void EncuentroPerdido()
    {
        if (EncuentroTerminado) return;
        EncuentroTerminado = true;

        PlayerTurn   = false;
        PlayerIATurn = false;

        Debug.Log("=== DERROTA: El jugador ha perdido el encuentro ===");
        Debug.Break(); // Pausa el editor; en build usa: Time.timeScale = 0f;
    }
}
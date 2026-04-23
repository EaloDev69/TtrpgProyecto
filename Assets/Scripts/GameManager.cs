using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float IniciativaParty;
    public float IniciativaEnemigos;
    public bool  PlayerTurn         = false;
    public bool  EncuentroTerminado = false;

    [Header("Configuracion")]
    public string escenaSiguiente     = "SiguienteEscena";
    public float  esperaEntreEnemigos = 1.5f;

    [Header("Party (arrastra aqui los PartyMember)")]
    public PartyMember[] party;

    [Header("Enemigos")]
    [SerializeField] private Enemigo[] enemigos;

    public Enemigo enemigoSeleccionado { get; private set; } = null;

    private int _indiceTurnoParty = 0;

    void Awake()
    {
        Instance           = this;
        IniciativaParty    = Random.Range(0, 10);
        IniciativaEnemigos = Random.Range(0, 10);
    }

    void Start()
    {
        if (party == null || party.Length == 0)
        {
            Debug.LogError("GameManager: el array 'party' está vacío.");
            return;
        }

        if (IniciativaEnemigos > IniciativaParty)
        {
            Debug.Log("Ganan iniciativa los enemigos.");
            IniciarTurnoEnemigo();
        }
        else
        {
            Debug.Log("Gana iniciativa la party.");
            IniciarTurnoJugador();
        }
    }

    public void SeleccionarEnemigo(Enemigo enemigo)
    {
        enemigoSeleccionado = enemigo;
    }

    private Enemigo ObtenerPrimerEnemigoVivo()
    {
        foreach (Enemigo e in enemigos)
            if (e != null && e.vivo) return e;
        return null;
    }

    public void CiclarEnemigo()
    {
        if (!PlayerTurn || EncuentroTerminado) return;
        if (enemigos == null || enemigos.Length == 0) return;

        int indiceActual = -1;
        for (int i = 0; i < enemigos.Length; i++)
        {
            if (enemigos[i] == enemigoSeleccionado)
            {
                indiceActual = i;
                break;
            }
        }

        int total = enemigos.Length;
        for (int offset = 1; offset <= total; offset++)
        {
            int i = (indiceActual + offset) % total;
            if (enemigos[i] != null && enemigos[i].vivo)
            {
                Debug.Log("Objetivo cambiado a: " + enemigos[i].nombreEnemigo);
                SeleccionarEnemigo(enemigos[i]);
                return;
            }
        }
    }

    public void IniciarTurnoJugador()
    {
        if (EncuentroTerminado) return;
        PlayerTurn = true;
        ResetearTurnoParty();
        Debug.Log("=== TURNO DEL JUGADOR ===");
    }

    public PartyMember ObtenerMiembroActual()
    {
        if (party == null || party.Length == 0) return null;
        if (_indiceTurnoParty >= 0 && _indiceTurnoParty < party.Length)
            return party[_indiceTurnoParty];
        return null;
    }

    public void AvanzarMiembroActual()
    {
        _indiceTurnoParty++;

        while (_indiceTurnoParty < party.Length &&
               (party[_indiceTurnoParty] == null || !party[_indiceTurnoParty].vivo))
            _indiceTurnoParty++;

        if (_indiceTurnoParty >= party.Length)
        {
            Debug.Log("Toda la party ha actuado.");
            IniciarTurnoEnemigo();
        }
        else
        {
            if (enemigoSeleccionado == null || !enemigoSeleccionado.vivo)
                enemigoSeleccionado = ObtenerPrimerEnemigoVivo();

            string objetivo = enemigoSeleccionado != null ? enemigoSeleccionado.nombreEnemigo : "ninguno";
            Debug.Log("Turno de: " + party[_indiceTurnoParty].nombrePersonaje +
                      " | Objetivo actual: " + objetivo);
        }
    }

    private void ResetearTurnoParty()
    {
        _indiceTurnoParty = 0;

        while (_indiceTurnoParty < party.Length &&
               (party[_indiceTurnoParty] == null || !party[_indiceTurnoParty].vivo))
            _indiceTurnoParty++;

        foreach (PartyMember miembro in party)
            if (miembro != null) miembro.yaActuo = false;

        enemigoSeleccionado = ObtenerPrimerEnemigoVivo();
        if (enemigoSeleccionado != null)
            Debug.Log("Objetivo inicial: " + enemigoSeleccionado.nombreEnemigo);

        if (_indiceTurnoParty < party.Length)
            Debug.Log("Turno de: " + party[_indiceTurnoParty].nombrePersonaje);
        else
            Debug.LogWarning("ResetearTurnoParty: ningún miembro vivo.");
    }

    public void ComprobarFinTurnoParty()
    {
        foreach (PartyMember miembro in party)
            if (miembro != null && miembro.vivo && !miembro.yaActuo) return;

        Debug.Log("Toda la party ha actuado. Turno del enemigo.");
        IniciarTurnoEnemigo();
    }

    public void IniciarTurnoEnemigo()
    {
        if (EncuentroTerminado) return;
        PlayerTurn = false;

        foreach (Enemigo e in enemigos)
            if (e != null) e.yaActuo = false;

        Debug.Log("=== TURNO DEL ENEMIGO ===");
        StartCoroutine(SecuenciaEnemigos());
    }

    private IEnumerator SecuenciaEnemigos()
    {
        foreach (Enemigo enemigo in enemigos)
        {
            if (EncuentroTerminado) yield break;

            if (enemigo == null || !enemigo.vivo)
            {
                if (enemigo != null) enemigo.yaActuo = true;
                continue;
            }

            enemigo.EjecutarTurno();
            yield return new WaitForSeconds(esperaEntreEnemigos);
        }

        if (!EncuentroTerminado)
        {
            Debug.Log("Todos los enemigos han actuado. Turno de la party.");
            IniciarTurnoJugador();
        }
    }

    public PartyMember ObtenerMiembroVivoAleatorio()
    {
        var vivos = new System.Collections.Generic.List<PartyMember>();
        foreach (PartyMember m in party)
            if (m != null && m.vivo) vivos.Add(m);

        if (vivos.Count == 0) return null;
        return vivos[Random.Range(0, vivos.Count)];
    }

    public void ComprobarVictoria()
    {
        foreach (Enemigo e in enemigos)
            if (e != null && e.vivo) return;
        EncuentroGanado();
    }

    public void ComprobarDerrota()
    {
        foreach (PartyMember m in party)
            if (m != null && m.vivo) return;
        EncuentroPerdido();
    }

    public void EncuentroGanado()
    {
        if (EncuentroTerminado) return;
        EncuentroTerminado = true;
        PlayerTurn         = false;
        Debug.Log("=== VICTORIA ===");
        SceneManager.LoadScene(escenaSiguiente);
    }

    public void EncuentroPerdido()
    {
        if (EncuentroTerminado) return;
        EncuentroTerminado = true;
        PlayerTurn         = false;
        Debug.Log("=== DERROTA ===");
        Debug.Break();
    }
}
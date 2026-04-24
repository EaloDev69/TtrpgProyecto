using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    // CAMBIO: eventos en vez de llamadas directas a la UI
    // BotonesAtaqueUI se suscribe a estos en su OnEnable/OnDisable
    public event Action OnPlayerTurnStarted;
    public event Action OnEnemyTurnStarted;
    public event Action OnBattleEnded;

    public float IniciativaParty;
    public float IniciativaEnemigos;
    public bool  PlayerTurn         = false;
    public bool  EncuentroTerminado = false;

    [Header("Configuracion")]
    public string escenaSiguiente     = "SiguienteEscena";
    public float  esperaEntreEnemigos = 1.5f;

    [Header("Party")]
    public PartyMember[] party;

    public Enemigo enemigoSeleccionado { get; private set; }

    private int _indiceTurnoParty = 0;

    // CAMBIO: listas cacheadas — se actualizan solo cuando alguien muere,
    // no se recorren completas en cada llamada
    private readonly List<PartyMember> _miembrosVivos  = new();
    private readonly List<Enemigo>     _enemigosVivos  = new();

    // CAMBIO: índice de enemigoSeleccionado para ciclar en O(1)
    private int _indiceEnemigoSeleccionado = 0;

    // -------------------------------------------------------------------------
    // Singleton robusto: sobrevive a recargas sin duplicar el objeto
    // -------------------------------------------------------------------------
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        IniciativaParty    = UnityEngine.Random.Range(0, 10);
        IniciativaEnemigos = UnityEngine.Random.Range(0, 10);

        // CAMBIO: construir las listas cacheadas una sola vez al arrancar
        RefrescarListasVivos();
    }

    void Start()
    {
        if (party == null || party.Length == 0)
        {
            Debug.LogError("BattleManager: el array 'party' está vacío.");
            return;
        }

        if (IniciativaEnemigos > IniciativaParty)
            IniciarTurnoEnemigo();
        else
            IniciarTurnoJugador();
    }

    // -------------------------------------------------------------------------
    // NUEVO: reconstruye ambas listas de vivos de una vez
    // Llama solo cuando alguien muere, no en cada frame o consulta
    // -------------------------------------------------------------------------
    public void RefrescarListasVivos()
    {
        _miembrosVivos.Clear();
        foreach (var m in party)
            if (m != null && m.vivo) _miembrosVivos.Add(m);

        _enemigosVivos.Clear();
        foreach (var e in enemigos)
            if (e != null && e.vivo) _enemigosVivos.Add(e);
    }

    // -------------------------------------------------------------------------
    // Selección de enemigo — ahora también guarda el índice
    // -------------------------------------------------------------------------
    public void SeleccionarEnemigo(Enemigo enemigo)
    {
        enemigoSeleccionado        = enemigo;
        _indiceEnemigoSeleccionado = _enemigosVivos.IndexOf(enemigo);
    }

    private Enemigo[] enemigos = Array.Empty<Enemigo>();

    public void SetEnemigos(Enemigo[] nuevosEnemigos)
    {
        enemigos = nuevosEnemigos;
        RefrescarListasVivos(); // ya tienes este método del refactor anterior
    }
    // CAMBIO: usa la lista cacheada + índice → sin bucle lineal
    public void CiclarEnemigo()
    {
        if (!PlayerTurn || EncuentroTerminado) return;
        if (_enemigosVivos.Count == 0) return;

        _indiceEnemigoSeleccionado =
            (_indiceEnemigoSeleccionado + 1) % _enemigosVivos.Count;

        enemigoSeleccionado = _enemigosVivos[_indiceEnemigoSeleccionado];
        Debug.Log("Objetivo cambiado a: " + enemigoSeleccionado.nombreEnemigo);
    }

    // -------------------------------------------------------------------------
    // Turno del jugador
    // -------------------------------------------------------------------------
    public void IniciarTurnoJugador()
    {
        if (EncuentroTerminado) return;
        PlayerTurn = true;
        ResetearTurnoParty();
        Debug.Log("=== TURNO DEL JUGADOR ===");

        // CAMBIO: evento en vez de llamada directa → UI desacoplada
        OnPlayerTurnStarted?.Invoke();
    }

    public PartyMember ObtenerMiembroActual() =>
        (_indiceTurnoParty < _miembrosVivos.Count)
            ? _miembrosVivos[_indiceTurnoParty]
            : null;

    public void AvanzarMiembroActual()
    {
        _indiceTurnoParty++;

        if (_indiceTurnoParty >= _miembrosVivos.Count)
        {
            Debug.Log("Toda la party ha actuado.");
            IniciarTurnoEnemigo();
        }
        else
        {
            // El objetivo siempre apunta a un vivo (lista ya filtrada)
            if (enemigoSeleccionado == null || !enemigoSeleccionado.vivo)
                SeleccionarPrimerEnemigo();

            Debug.Log($"Turno de: {_miembrosVivos[_indiceTurnoParty].nombrePersonaje}" +
                      $" | Objetivo: {enemigoSeleccionado?.nombreEnemigo ?? "ninguno"}");
            OnPlayerTurnStarted?.Invoke();
            BotonesAtaqueUI.Instance?.RefrescarBotones();
        }
    }

    private void ResetearTurnoParty()
    {
        _indiceTurnoParty = 0;

        foreach (var m in party)
            if (m != null) m.yaActuo = false;

        SeleccionarPrimerEnemigo();

        if (_miembrosVivos.Count > 0)
            Debug.Log("Turno de: " + _miembrosVivos[0].nombrePersonaje);
        else
            Debug.LogWarning("ResetearTurnoParty: ningún miembro vivo.");
    }

    private void SeleccionarPrimerEnemigo()
    {
        if (_enemigosVivos.Count > 0)
        {
            _indiceEnemigoSeleccionado = 0;
            enemigoSeleccionado = _enemigosVivos[0];
            Debug.Log("Objetivo: " + enemigoSeleccionado.nombreEnemigo);
        }
        else
        {
            enemigoSeleccionado = null;
        }
    }

    // CAMBIO: ComprobarFinTurnoParty ahora usa la lista cacheada
    public void ComprobarFinTurnoParty()
    {
        foreach (var m in _miembrosVivos)
            if (!m.yaActuo) return;

        Debug.Log("Toda la party ha actuado. Turno del enemigo.");
        IniciarTurnoEnemigo();
    }

    // -------------------------------------------------------------------------
    // Turno del enemigo
    // -------------------------------------------------------------------------
    public void IniciarTurnoEnemigo()
    {
        if (EncuentroTerminado) return;
        PlayerTurn = false;

        foreach (var e in enemigos)
            if (e != null) e.yaActuo = false;

        Debug.Log("=== TURNO DEL ENEMIGO ===");
        OnEnemyTurnStarted?.Invoke();
        StartCoroutine(SecuenciaEnemigos());
    }

    // CAMBIO: itera _enemigosVivos (ya filtrado) en vez del array completo
    private IEnumerator SecuenciaEnemigos()
    {
        // Snapshot para no mutar la lista mientras la recorremos
        var snapshot = new List<Enemigo>(_enemigosVivos);

        foreach (var enemigo in snapshot)
        {
            if (EncuentroTerminado) yield break;
            enemigo.EjecutarTurno();
            yield return new WaitForSeconds(esperaEntreEnemigos);
        }

        if (!EncuentroTerminado)
        {
            Debug.Log("Todos los enemigos han actuado. Turno de la party.");
            IniciarTurnoJugador();
        }
    }

    // CAMBIO: usa la lista cacheada → sin crear List<> nueva cada vez
    public PartyMember ObtenerMiembroVivoAleatorio()
    {
        if (_miembrosVivos.Count == 0) return null;
        return _miembrosVivos[UnityEngine.Random.Range(0, _miembrosVivos.Count)];
    }

    // -------------------------------------------------------------------------
    // CAMBIO: comprobar victoria/derrota ahora es O(1) gracias a las listas
    // Llama a RefrescarListasVivos() desde el script del muerto antes de aquí
    // -------------------------------------------------------------------------
    public void ComprobarVictoria()
    {
        if (_enemigosVivos.Count == 0) EncuentroGanado();
    }

    public void ComprobarDerrota()
    {
        if (_miembrosVivos.Count == 0) EncuentroPerdido();
    }

    // -------------------------------------------------------------------------
    // Fin de combate
    // -------------------------------------------------------------------------
    public void EncuentroGanado()
    {
        if (EncuentroTerminado) return;
        EncuentroTerminado = true;
        PlayerTurn         = false;
        OnBattleEnded?.Invoke();
        PotionSystem.Instance?.AgregarPocion(4);
        Debug.Log("=== VICTORIA ===");
        SceneManager.LoadScene(escenaSiguiente);
    }

    public void EncuentroPerdido()
    {
        if (EncuentroTerminado) return;
        EncuentroTerminado = true;
        PlayerTurn         = false;
        OnBattleEnded?.Invoke();
        Debug.Log("=== DERROTA ===");
        Debug.Break();
    }
}
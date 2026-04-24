// PotionSystem.cs
using UnityEngine;

public class PotionSystem : MonoBehaviour
{
    public static PotionSystem Instance { get; private set; }

    [Header("Configuracion")]
    public int   potionesIniciales = 3;
    public int   maximoPotiones    = 9;  // asignable cuando te confirmen
    public float cantidadCura      = 30f; // asignable cuando te confirmen

    [Header("Salud maxima por defecto")]
    public float saludMaxima = 100f;

    public int PotionesActuales { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject); // persiste entre escenas

        // Si es la primera vez, inicializar con las pociones base
        if (!PlayerPrefs.HasKey("potiones"))
            PlayerPrefs.SetInt("potiones", potionesIniciales);

        PotionesActuales = Mathf.Clamp(
            PlayerPrefs.GetInt("potiones"),
            0,
            maximoPotiones
        );
    }

    // Llamar al derrotar un enemigo
    public void AgregarPocion(int cantidad = 1)
    {
        PotionesActuales = Mathf.Min(PotionesActuales + cantidad, maximoPotiones);
        Guardar();
        Debug.Log($"Pociones: {PotionesActuales}/{maximoPotiones}");
    }

    // Devuelve false si no hay pociones o el miembro ya está al máximo
    public bool UsarPocion()
    {
        if (PotionesActuales <= 0)
        {
            Debug.Log("No quedan pociones.");
            return false;
        }

        PartyMember miembro = BattleManager.Instance?.ObtenerMiembroActual();
        if (miembro == null || !miembro.vivo)
        {
            Debug.Log("No hay miembro activo.");
            return false;
        }

        float saludMax = miembro.saludMaxima; // propiedad que agregaremos a PartyMember
        if (miembro.salud >= saludMax)
        {
            Debug.Log($"{miembro.nombrePersonaje} ya tiene la salud al máximo.");
            return false;
        }

        // Curar
        float saludAntes  = miembro.salud;
        miembro.salud     = Mathf.Min(miembro.salud + cantidadCura, saludMax);
        float curado      = miembro.salud - saludAntes;
        PotionesActuales--;

        Guardar();
        Debug.Log($"{miembro.nombrePersonaje} curado en {curado} HP. " +
                  $"Salud: {miembro.salud}/{saludMax} | Pociones: {PotionesActuales}");

        // Consumir turno igual que un ataque
        miembro.yaActuo = true;
        BattleManager.Instance.AvanzarMiembroActual();

        // Avisar a la UI
        BotonesAtaqueUI.Instance?.RefrescarBotones();

        return true;
    }

    void Guardar() => PlayerPrefs.SetInt("potiones", PotionesActuales);
}
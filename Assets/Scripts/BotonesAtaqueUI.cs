using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotonesAtaqueUI : MonoBehaviour
{
    public static BotonesAtaqueUI Instance { get; private set; }

    [Header("Botones de ataque (en orden 0-1-2)")]
    public Button[] botonesAtaque;

    [Header("Boton de pocion")]
    public Button            botonPocion;
    public TextMeshProUGUI   textoPocion; // texto del botón, ej: "Poción (3)"

    void Awake() => Instance = this;

    void Start() => RefrescarBotones();

    public void RefrescarBotones()
    {
        RefrescarBotonesAtaque();
        RefrescarBotonPocion();
    }

    void RefrescarBotonesAtaque()
    {
        PartyMember miembro = BattleManager.Instance != null
            ? BattleManager.Instance.ObtenerMiembroActual()
            : null;

        for (int i = 0; i < botonesAtaque.Length; i++)
        {
            if (botonesAtaque[i] == null) continue;

            bool tieneAtaque = miembro != null
                            && miembro.datos != null
                            && i < miembro.datos.ataques.Length;

            botonesAtaque[i].gameObject.SetActive(tieneAtaque);

            if (!tieneAtaque) continue;

            AtaqueDato atq = miembro.datos.ataques[i];
            var tmpro = botonesAtaque[i].GetComponentInChildren<TextMeshProUGUI>();
            if (tmpro != null)
                tmpro.text = $"{atq.nombreAtaque}\n({atq.dañoMin}–{atq.dañoMax})";
            else
            {
                var legacy = botonesAtaque[i].GetComponentInChildren<Text>();
                if (legacy != null)
                    legacy.text = $"{atq.nombreAtaque} ({atq.dañoMin}-{atq.dañoMax})";
            }
        }
    }

    void RefrescarBotonPocion()
    {
        if (botonPocion == null) return;

        bool esturnoJugador = BattleManager.Instance != null
                           && BattleManager.Instance.PlayerTurn
                           && !BattleManager.Instance.EncuentroTerminado;

        int pociones = PotionSystem.Instance != null
            ? PotionSystem.Instance.PotionesActuales
            : 0;

        // Mostrar solo en turno del jugador
        botonPocion.gameObject.SetActive(esturnoJugador);

        // Desactivar si no quedan pociones
        botonPocion.interactable = pociones > 0;

        if (textoPocion != null)
            textoPocion.text = $"Poción ({pociones})";
    }

    // Llama esto desde el OnClick del botón en el Inspector
    public void OnClickPocion()
    {
        if (!BattleManager.Instance.PlayerTurn) return;
        PotionSystem.Instance?.UsarPocion();
    }
}
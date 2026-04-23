using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gestiona la visibilidad y el texto de los botones de ataque según
/// los ataques disponibles del miembro de party activo.
///
/// Setup:
///   1. Crea hasta 3 botones de ataque en tu Canvas.
///   2. Arrastra cada botón al array botonesAtaque en el Inspector.
///   3. En el OnClick de cada botón llama a Ataque.BotonAtacar(0/1/2).
///   4. Llama a RefrescarBotones() cada vez que cambie el miembro activo
///      (puedes hacerlo desde GameManager.IniciarTurnoJugador o similar).
/// </summary>
public class BotonesAtaqueUI : MonoBehaviour
{
    public static BotonesAtaqueUI Instance { get; private set; }

    [Header("Botones de ataque (arrastra aquí, en orden 0-1-2)")]
    public Button[] botonesAtaque;    // Asigna hasta 3 botones en el Inspector

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RefrescarBotones();
    }

    /// <summary>
    /// Muestra solo los botones que corresponden a ataques del miembro actual.
    /// Actualiza el texto del botón con el nombre del ataque.
    /// </summary>
    public void RefrescarBotones()
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

            if (tieneAtaque)
            {
                AtaqueDato atq = miembro.datos.ataques[i];

                // Actualiza el texto del botón (funciona con TextMeshPro y con UI Text)
                var tmpro = botonesAtaque[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (tmpro != null)
                {
                    tmpro.text = $"{atq.nombreAtaque}\n({atq.dañoMin}–{atq.dañoMax})";
                }
                else
                {
                    var legacyText = botonesAtaque[i].GetComponentInChildren<Text>();
                    if (legacyText != null)
                        legacyText.text = $"{atq.nombreAtaque} ({atq.dañoMin}-{atq.dañoMax})";
                }
            }
        }
    }
}

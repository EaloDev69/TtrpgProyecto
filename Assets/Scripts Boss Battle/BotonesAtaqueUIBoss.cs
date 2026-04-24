using UnityEngine;
using UnityEngine.UI;


public class BotonesAtaqueUIBoss : MonoBehaviour
{
    public static BotonesAtaqueUIBoss Instance { get; private set; }

    [Header("Botones de ataque (arrastra en orden: 0, 1, 2...)")]
    [SerializeField] private Button[] botonesAtaque;

    void Awake()
    {
        Instance = this;
    }
    
    public void RefrescarBotones()
    {
        if (botonesAtaque == null || botonesAtaque.Length == 0)
        {
            Debug.LogWarning("BotonesAtaqueUIBoss: no hay botones asignados en el Inspector.");
            return;
        }

        PartyMemberBoss miembro = BattleManagerBoss.Instance != null
            ? BattleManagerBoss.Instance.ObtenerMiembroActual()
            : null;

        // Si no hay miembro válido, ocultamos todos
        if (miembro == null || miembro.datos == null)
        {
            OcultarTodos();
            return;
        }

        int cantidadAtaques = miembro.datos.ataques.Length;

        for (int i = 0; i < botonesAtaque.Length; i++)
        {
            if (botonesAtaque[i] == null) continue;

            bool tieneEsteAtaque = i < cantidadAtaques;
            botonesAtaque[i].gameObject.SetActive(tieneEsteAtaque);

            // Actualiza el texto del botón con el nombre del ataque
            if (tieneEsteAtaque)
            {
                Text textoBoton = botonesAtaque[i].GetComponentInChildren<Text>();
                if (textoBoton != null)
                    textoBoton.text = miembro.datos.ataques[i].nombreAtaque;
            }
        }

        Debug.Log($"BotonesAtaqueUIBoss: mostrando {cantidadAtaques} botones para {miembro.nombrePersonaje}.");
    }

    private void OcultarTodos()
    {
        foreach (Button b in botonesAtaque)
            if (b != null) b.gameObject.SetActive(false);
    }
}

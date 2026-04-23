using UnityEngine;
using TMPro;        


public class TurnoUI : MonoBehaviour
{
    [Header("Referencia al texto de turno")]
    public TextMeshProUGUI etiquetaTurno;   
    void Update()
    {
        ActualizarEtiqueta();
    }

    private void ActualizarEtiqueta()
    {
        if (etiquetaTurno == null || GameManager.Instance == null) return;

        if (GameManager.Instance.EncuentroTerminado)
        {
            etiquetaTurno.text = "";
            return;
        }

        if (GameManager.Instance.PlayerTurn)
        {
            PartyMember actual = GameManager.Instance.ObtenerMiembroActual();

            if (actual != null)
                etiquetaTurno.text = "Turno de: " + actual.nombrePersonaje;
            else
                etiquetaTurno.text = "";
        }
        else
        {
            etiquetaTurno.text = "Turno de los enemigos...";
        }
    }
}
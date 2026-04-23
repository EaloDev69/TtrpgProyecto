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
        if (etiquetaTurno == null || BattleManager.Instance == null) return;

        if (BattleManager.Instance.EncuentroTerminado)
        {
            etiquetaTurno.text = "";
            return;
        }

        if (BattleManager.Instance.PlayerTurn)
        {
            PartyMember actual = BattleManager.Instance.ObtenerMiembroActual();

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
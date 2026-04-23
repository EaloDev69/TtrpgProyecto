using UnityEngine;

public class PartyMember : MonoBehaviour
{
    public string nombrePersonaje = "Personaje";
    public bool   yaActuo         = false;
    public float  salud           = 100f;
    public bool   vivo            = true;
    public void RecibirDaño(float cantidad)
    {
        if (!vivo) return;

        salud = Mathf.Max(0f, salud - cantidad);
        Debug.Log(nombrePersonaje + " recibió " + cantidad + " de daño. Salud: " + salud);

        if (salud <= 0f)
        {
            vivo    = false;
            yaActuo = true;
            Debug.Log(nombrePersonaje + " ha muerto.");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ComprobarDerrota();
                if (!GameManager.Instance.EncuentroTerminado)
                    GameManager.Instance.ComprobarFinTurnoParty();
            }
        }
    }
}
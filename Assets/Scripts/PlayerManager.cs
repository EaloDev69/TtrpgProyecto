using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public float salud = 100f;

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
        }
    }

    void Update() { }

    public void RecibirDaño(float cantidad)
    {
        salud -= cantidad;
        Debug.Log("Jugador recibió " + cantidad + " de daño. Salud restante: " + salud);

        if (salud <= 0)
        {
            salud = 0;
            if (GameManager.Instance != null)
                GameManager.Instance.EncuentroPerdido();
        }
    }
}
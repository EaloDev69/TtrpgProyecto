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

    void Update()
    {
        if (salud <= 0)
        {
            Debug.Log("El jugador ha muerto.");
            Destroy(gameObject);
        }
    }

    public void RecibirDaño(float cantidad)
    {
        salud -= cantidad;
        Debug.Log("Jugador recibió " + cantidad + " de daño. Salud restante: " + salud);
    }
}

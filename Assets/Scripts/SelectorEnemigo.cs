using UnityEngine;

public class SelectorEnemigo : MonoBehaviour
{
    public static SelectorEnemigo Instance { get; private set; }

    [Header("Prefijo del texto (opcional)")]
    [SerializeField] private string prefijo = "Objetivo: ";

    void Awake()
    {
        Instance = this;
    }

    public void BotonCiclarEnemigo()
    {
        GameManager.Instance.CiclarEnemigo();
    }
}

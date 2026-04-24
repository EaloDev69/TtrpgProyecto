using UnityEngine;
using UnityEngine.SceneManagement;


public class EscenasManagerBoss : MonoBehaviour
{
    public static EscenasManagerBoss Instance { get; private set; }

    [Header("Nombres de escena (deben coincidir con el Build Settings)")]
    [SerializeField] private string escenaVictoria = "EscenaVictoria_Placeholder";
    [SerializeField] private string escenaDerrota  = "EscenaDerrota_Placeholder";

    void Awake()
    {
        Instance = this;
    }

    public void IrAVictoria()
    {
        Debug.Log($"[EscenasManagerBoss] Cargando escena de victoria: {escenaVictoria}");
        SceneManager.LoadScene(escenaVictoria);
    }

   
    public void IrADerrota()
    {
        Debug.Log($"[EscenasManagerBoss] Cargando escena de derrota: {escenaDerrota}");
        SceneManager.LoadScene(escenaDerrota);
    }
}

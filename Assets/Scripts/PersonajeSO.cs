using UnityEngine;

[CreateAssetMenu(fileName = "Personaje_Nuevo", menuName = "TTRPG/Personaje")]
public class PersonajeSO : ScriptableObject
{
    [Header("Identidad")]
    public string nombrePersonaje = "Personaje";

    [Header("Estadísticas base")]
    public float saludMaxima = 100f;

    [Header("Ataques (máximo 3)")]
    [Tooltip("Agrega entre 1 y 3 ataques. El orden define los botones en pantalla.")]
    public AtaqueDato[] ataques = new AtaqueDato[1];

    // Validación en el editor: no permite más de 3 ataques
    private void OnValidate()
    {
        if (ataques != null && ataques.Length > 3)
        {
            Debug.LogWarning($"[{name}] Solo se permiten hasta 3 ataques. Recorta el array.");
        }
    }
}

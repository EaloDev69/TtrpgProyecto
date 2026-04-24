using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        public string      nombre;
        public GameObject  prefab;
        [Range(1, 4)]
        public int         minCantidad = 1;
        [Range(1, 4)]
        public int         maxCantidad = 4;
    }

    [Header("Grupos de enemigos disponibles")]
    public EnemyGroup[] gruposEnemigos;

    [Header("Posiciones de spawn (arrastra los transforms aqui)")]
    public Transform[] puntosDeSpawn; // 4 puntos en la escena

    [Header("Referencia")]
    public BattleManager battleManager;

    void Awake()
    {
        // Spawner corre antes que BattleManager (Script Execution Order)
        // o simplemente ponlo en Awake y BattleManager en Start
        SpawnGrupo();
    }

    void SpawnGrupo()
    {
        if (gruposEnemigos == null || gruposEnemigos.Length == 0)
        {
            Debug.LogError("EnemySpawner: no hay grupos configurados.");
            return;
        }

        // 1. Elegir grupo al azar
        EnemyGroup grupo = gruposEnemigos[Random.Range(0, gruposEnemigos.Length)];

        // 2. Elegir cantidad
        int cantidad = Random.Range(grupo.minCantidad, grupo.maxCantidad + 1);
        cantidad = Mathf.Clamp(cantidad, 1, puntosDeSpawn.Length); // mínimo siempre 1

        // 3. Instanciar y registrar en BattleManager
        var enemigosInstanciados = new Enemigo[cantidad];

        for (int i = 0; i < cantidad; i++)
        {
            GameObject go = Instantiate(
                grupo.prefab,
                puntosDeSpawn[i].position,
                puntosDeSpawn[i].rotation
            );

            // Nombre con número si hay más de uno
            Enemigo e = go.GetComponent<Enemigo>();
            if (cantidad > 1)
                e.nombreEnemigo = $"{grupo.nombre} {IntToRomano(i + 1)}";
            else
                e.nombreEnemigo = grupo.nombre;

            enemigosInstanciados[i] = e;
        }

        // 4. Pasarle el array al BattleManager
        // (el campo 'enemigos' debe ser público o tener un método setter)
        battleManager.SetEnemigos(enemigosInstanciados);

        Debug.Log($"Spawneados {cantidad}x {grupo.nombre}");
    }

    // I, II, III, IV — más legible que "Goblin 1"
    private string IntToRomano(int n) => n switch
    {
        1 => "I", 2 => "II", 3 => "III", 4 => "IV", _ => n.ToString()
    };
    void OnValidate()
    {
        if (gruposEnemigos == null) return;
        foreach (var grupo in gruposEnemigos)
        {
            grupo.minCantidad = Mathf.Max(1, grupo.minCantidad); // nunca menor a 1
            grupo.maxCantidad = Mathf.Max(grupo.minCantidad, grupo.maxCantidad); // max >= min
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs enemigos")]
    [SerializeField] public GameObject[] enemyPrefabs;
    [SerializeField] public Transform    spawnEnemigo;
    
    private int cantidadEnemigos;

    private List<GameObject> enemigosActivos = new List<GameObject>();

    // BattleManager llama esto desde InitBattle()
    public void SpawnEnemigos()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("EnemySpawner: no hay prefabs de enemigos asignados.");
            return;
        }

        for (int i = 0; i < cantidadEnemigos; i++)
        {
            int index = Random.Range(0, enemyPrefabs.Length);
            GameObject obj = Instantiate(enemyPrefabs[index], spawnEnemigo.position, Quaternion.identity);

            Enemigo componente = obj.GetComponent<Enemigo>();
            if (componente != null)
            {
                enemigosActivos.Add(obj);
                // Se registra directamente en BattleManager
                BattleManager.Instance.RegistrarEnemigo(componente);
                Debug.Log("EnemySpawner: spawneado y registrado -> " + componente.nombreEnemigo);
            }
            else
            {
                Debug.LogWarning("EnemySpawner: el prefab en índice " + index + " no tiene componente Enemigo.");
                Destroy(obj);
            }
        }
    }
    void Awake()
    {
        cantidadEnemigos = Random.Range(1, 5); // 1 a 4 inclusive
        Debug.Log("EnemySpawner: enemigos en este encuentro -> " + cantidadEnemigos);
    }
}

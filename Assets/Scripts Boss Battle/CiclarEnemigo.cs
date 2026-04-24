using UnityEngine;

public class CiclarEnemigo : MonoBehaviour
{
    public static CiclarEnemigo Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void BotonCiclarEnemigo()
    {
        BattleManagerBoss.Instance.CiclarEnemigo();
    }
}

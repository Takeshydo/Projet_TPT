using UnityEngine;

public class CleanEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var WolrdEnemy = GetComponent<EnemyID>();
        if (WolrdEnemy == null) return;

        if (GameManagement.Instance.defeatedEnemies.Contains(WolrdEnemy.enemyID))
        {
            Destroy(gameObject);
        }
    }
}

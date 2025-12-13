using TreeEditor;
using UnityEngine;

public class CameraFigthing : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Player;
    public Transform PlayerT;
    public Transform EnemyT;
    public float height = 2f;
    public float distance = 5f;
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if(!PlayerT || !EnemyT)return;
        Vector3 FightDirection = (EnemyT.position - PlayerT.position).normalized;

        Vector3 desiredPosition=PlayerT.position - FightDirection * distance + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        Vector3 LookTarget= PlayerT.position +FightDirection * 2f;
        transform.LookAt(LookTarget);

    }

    public void SetNewEnemy(GameObject NewEnemyInstance)
    {
        Enemy=NewEnemyInstance;
        EnemyT=NewEnemyInstance.transform;
    }
    public void SetNewHero(GameObject NewHeroInstance)
    {
        Player=NewHeroInstance;
        PlayerT=NewHeroInstance.transform;
    }

}

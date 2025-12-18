using UnityEngine;

public class TurnBaseManager : MonoBehaviour
{
    private GameObject Player;
    private GameObject Enemy;

    private Enemy enemyScript;
    private Action heroAction;

    void Start()
    {
        StartCombat();
    }
    void StartCombat()
    {
        heroAction.StartTurn();
        Debug.Log("Début du tour joueur");
        //enemyScript.StartTurn();
    }

    void EndCombat()
    {
        if(enemyScript.IsDead)
        {
            Debug.Log("Fin du Combat | Hero Vainqueur");
        }
        if (heroAction.IsDead)
        {
            Debug.Log("Fin du combat | Enemy Vainqueur");
        }

    }

    public void SetNewEnemy(GameObject NewEnemyInstance)
    {
        Enemy=NewEnemyInstance;
        if(Enemy != null)
        {
            enemyScript = Enemy.GetComponent<Enemy>();
        }
    }

        public void SetNewHero(GameObject NewHeroInstance)
    {
        Player=NewHeroInstance;
        if(Player != null)
        {
            heroAction = Player.GetComponent<Action>();
        } 
        else
        {
            Debug.Log("Player est null");
        }      
    }  
}

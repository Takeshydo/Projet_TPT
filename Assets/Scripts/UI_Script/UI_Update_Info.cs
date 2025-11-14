using UnityEngine;
using UnityEngine.UI;

public class UI_Update_Info : MonoBehaviour
{
    public Text heronametext;
    public Text heroleveltext;

    public Text enemynametext;
    public Text enemyleveltext;

    public GameObject hero;
    public GameObject enemy;

    void Start()
    {
        // On récupère les instances dans la scène
        if (hero == null)
            hero = GameObject.FindGameObjectWithTag("Player");
        if (enemy == null)
            enemy = GameObject.FindGameObjectWithTag("Enemy");

        UpdateCharacterUI();
    }

    public void UpdateCharacterUI()
    {
        if (hero == null || enemy == null)
            return;

        Hero heroStats = hero.GetComponent<Hero>();
        Enemy enemyStats = enemy.GetComponent<Enemy>();

        heronametext.text = heroStats.HeroName;
        heroleveltext.text = heroStats.HeroLevel.ToString();

        enemynametext.text = enemyStats.EnemyName;
        enemyleveltext.text = enemyStats.EnemyLvl.ToString();
    }
}

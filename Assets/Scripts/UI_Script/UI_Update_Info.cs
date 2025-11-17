using UnityEngine;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using JetBrains.Annotations;

public class UI_Update_Info : MonoBehaviour
{
    public TextMeshProUGUI heronametext;
    public TextMeshProUGUI heroleveltext;

    public TextMeshProUGUI enemynametext;
    public TextMeshProUGUI enemyleveltext;
    public TextMeshProUGUI HeroHP;

    public Scrollbar lifebar;
    public Button MoveButton;
    public Button AttackButton;
    public Button ObjectButton;

    public GameObject hero;
    public GameObject enemy;


    void Start()
    {
        if (hero == null)
            hero = GameObject.FindGameObjectWithTag("Player");
        if (enemy == null)
            enemy = GameObject.FindGameObjectWithTag("Enemy");

        UpdateCharacterUI();
        HPXPUI();

    }

    void Update()
    {
        HPXPUI();
        ButtonKeyInput();
    }

    public void UpdateCharacterUI()
    {
        if (hero == null || enemy == null)
            return;

        Hero heroStats = hero.GetComponent<Hero>();
        Enemy enemyStats = enemy.GetComponent<Enemy>();

        heronametext.text = heroStats.HeroName;
        heroleveltext.text = heroStats.HeroLevel.ToString();
        heroleveltext.text = $"lvl {heroStats.HeroLevel}";
        enemyleveltext.text = $"lvl {enemyStats.EnemyLvl}";
    }

    public void HPXPUI()
    {
        Hero heroStats = hero.GetComponent<Hero>();
        float ratioHP = heroStats.currentHP / heroStats.MaxHP; // la vie en pourcentage pour l'affichage
        float ratioXP = heroStats.currentXP / heroStats.MaxXP;

        HeroHP.text = $"{heroStats.currentHP} HP";
        lifebar.size = ratioHP;

    }

    public void ButtonKeyInput()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            AttackButton.onClick.Invoke();
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            MoveButton.onClick.Invoke();
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            ObjectButton.onClick.Invoke();
        }
    }
}

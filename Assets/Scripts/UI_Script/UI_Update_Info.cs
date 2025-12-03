using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using System.Collections;

public class UI_Update_Info : MonoBehaviour
{
    public TextMeshProUGUI heronametext;
    public TextMeshProUGUI heroleveltext;

    public TextMeshProUGUI enemynametext;
    public TextMeshProUGUI enemyleveltext;
    public TextMeshProUGUI HeroHP;
    public TextMeshProUGUI PlacementNameD;
    public TextMeshProUGUI PlacementNameQ;

    public Scrollbar lifebar;
    public Scrollbar xpbar;
    public Button MoveButton;
    public Button AttackButton;
    public Button ObjectButton;
    public Button MoveRightB;
    public Button MoveLeftB;

    public GameObject hero;
    public GameObject enemy;
    private Action HeroAction;
    public SkillButtonGenerator skillBG;
    public CanvasGroup ActionUI;
    public CanvasGroup AttackUI;
    public CanvasGroup MoveUI;
    public CanvasGroup ObjectUI;
    public CanvasGroup ReturnUI;

    // Variables d'identification
    private bool A_Attack = false;
    private bool A_Object = false;
    private bool A_Move = false;
    private bool A_ActionM = true;
    private bool SkillGen = false;
    private bool hasMoved = false;

    private bool canProcessInput = true;

    void Start()
    {
        if (hero == null)
        {
            hero = GameObject.FindGameObjectWithTag("Player");
        }
        if (enemy == null)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
        }
        HeroAction = hero.GetComponent<Action>();
        skillBG = AttackUI.GetComponent<SkillButtonGenerator>();
        UpdateCharacterUI();
        HPXPUI();
        MoveUIChange();
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
        heroleveltext.text = $"lvl {heroStats.HeroLevel}";
        enemyleveltext.text = $"lvl {enemyStats.EnemyLvl}";
    }

    public void HPXPUI()
    {
        Hero heroStats = hero.GetComponent<Hero>();
        float ratioHP = heroStats.currentHP / heroStats.MaxHP;
        float ratioXP = heroStats.currentXP / heroStats.MaxXP;

        HeroHP.text = $"{heroStats.currentHP} HP";
        lifebar.size = ratioHP;
        xpbar.size = ratioXP;
    }

    public void ButtonKeyInput()
    {
        if (!canProcessInput) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame && A_ActionM && canProcessInput)
        {
            AttackButton.onClick.Invoke();
            A_Attack = true;
            A_ActionM = false;
            ActionUIFade();
            if (!SkillGen)
            {
                skillBG.GenerateSkillButtons();
                SkillGen = true;
            }

            StartCoroutine(InputDelay(0.8f));

        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame && !hasMoved && A_ActionM)
        {
            MoveButton.onClick.Invoke();
            A_Move = true;
            ActionUIFade();
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame && A_ActionM)
        {
            ObjectButton.onClick.Invoke();
            A_Object = true;
            ActionUIFade();
        }
        // Regarde les Inputs
        if (A_Move)
        {
            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                HeroAction.MoveRight();
                MoveUIChange();
                ActionMenu();
                hasMoved = true;
            }
            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                HeroAction.MoveLeft();
                MoveUIChange();
                ActionMenu();
                hasMoved = true;
            }
        }
        if (A_Attack)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                Debug.Log("Attack 1");
            }
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                Debug.Log("Attack 2");
            }
            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                Debug.Log("Attack 3");
            }
        }



        // Retour au menu principal avec Ctrl
        if ((A_Attack || A_Move || A_Object) && Keyboard.current.ctrlKey.wasPressedThisFrame)
        {
            ActionMenu();
            Debug.Log("Retour au Menu");
        }
    }

    public void ActionUIFade()
    {
        // Masquer ActionUI
        ActionUI.alpha = 0f;
        ActionUI.interactable = false;
        ActionUI.blocksRaycasts = false;

        // Montrer le bon panel
        if (A_Move)
        {
            MoveUI.alpha = 1f;
            MoveUI.interactable = false;
            MoveUI.blocksRaycasts = false;
        }
        else if (A_Attack)
        {
            AttackUI.alpha = 1f;
            AttackUI.interactable = false;
            AttackUI.blocksRaycasts = false;
        }
        else if (A_Object)
        {
            ObjectUI.alpha = 1f;
            ObjectUI.interactable = false;
            ObjectUI.blocksRaycasts = false;
        }

        if (A_Move || A_Attack || A_Object)
        {
            ReturnUI.alpha = 1f;
            ReturnUI.interactable = false;
            ReturnUI.blocksRaycasts = false;
        }
        A_ActionM = false;
    }

    public void ActionMenu()
    {

        ActionUI.alpha = 1f;
        ActionUI.interactable = false;
        ActionUI.blocksRaycasts = true;

        AttackUI.alpha = 0f;
        AttackUI.interactable = false;
        AttackUI.blocksRaycasts = false;

        MoveUI.alpha = 0f;
        MoveUI.interactable = false;
        MoveUI.blocksRaycasts = false;

        ObjectUI.alpha = 0f;
        ObjectUI.interactable = false;
        ObjectUI.blocksRaycasts = false;

        ReturnUI.alpha = 0f;
        ReturnUI.interactable = false;
        ReturnUI.blocksRaycasts = false;


        A_Attack = A_Move = A_Object = SkillGen = false;
        A_ActionM = true;

    }

    /* public void UpdatePlacementName()
     {


     }*/

    public void MoveUIChange()
    {
        PlacementNameD.text = HeroAction.GetRightZone();
        PlacementNameQ.text = HeroAction.GetLeftZone();
    }
    private IEnumerator InputDelay(float delay)
    {
        canProcessInput = false;
        yield return new WaitForSeconds(delay);
        canProcessInput = true;

    }
}

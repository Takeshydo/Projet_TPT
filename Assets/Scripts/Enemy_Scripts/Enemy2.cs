using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Enemy2 : MonoBehaviour
{
    //Var Donnée du Monstre
    public string EnemyName = "Enemy2";
    public int EnemyLvl = 5;
    public float EnemyHP = 20.0f;
    public float EnemyXP = 30f;
    [SerializeField] private float CEnemyHP;
    public float Damage = 5.0f;
    public float Defense = 5.0f;
    public string status = "None";
    public float speed = 5f;

    public event System.Action<Enemy2> OnDeath;
    public Action heroAction;
    private CombatManager combatManager;
    [SerializeField] private Pig_Skill_Table pig_Skill;

    private bool EnemyIsDead => CEnemyHP <= 0;
    public bool hasFinished { get; private set; }
    void Start()
    {
        CEnemyHP = EnemyHP;
        status = "None";

        combatManager = FindFirstObjectByType<CombatManager>();

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TakeDamage(float amount)
    {
        float reductionratio = Defense / 100; // Passage defense en % 
        reductionratio = Mathf.Min(reductionratio, 1.0f); //Verif du ration < 100%
        float DamageTaken = amount * (1f - reductionratio); //Dégat avec ratio def
        DamageTaken = Mathf.Max(DamageTaken, 0f); //Verif du ration > 0
        CEnemyHP -= DamageTaken;
        Debug.Log("Enemy HP" + CEnemyHP + "");
        if (CEnemyHP <= 0)
        {
            Die();
        }
    }

    public void StartTurn()
    {
        hasFinished = false;
        Attack();
    }

    void Die()
    {
        Debug.Log("Enemie est mort");
        OnDeath?.Invoke(this);
    }

    void Attack()
    {
        if (heroAction == null) return;
        ZoneArea.ZoneType heroZone = ZoneArea.TagToZoneType(heroAction.GetCurrentZone());
        var possibleAttack = pig_Skill.Skills
            .Where(a => a.validZones.Contains(heroZone))
            .ToList();

        if (possibleAttack.Count == 0)
        {
            Debug.Log("Aucune Attaque possible");
            EndTurn();
            return;
        }
        int randomIndex = Random.Range(0, possibleAttack.Count);
        Pig_Skill chosenSkill = ChoosedAttack(possibleAttack);
        ExecuteAttack(chosenSkill);
    }

    Pig_Skill ChoosedAttack(List<Pig_Skill> attacks)
    {
        float Totalweight = attacks.Sum(a => a.Proba);
        float roll = Random.value * Totalweight;

        float current = 0;
        foreach (var attack in attacks)
        {
            Debug.Log($"Skill {attack.AttackName}, validZones: {string.Join(", ", attack.validZones)}");
            current += attack.Proba;
            if (roll <= current)
            {
                return attack;
            }
        }
        return attacks[0];
    }

    void ExecuteAttack(Pig_Skill attack)
    {
        Debug.Log($"L'ennemi utilise {attack.AttackName}");

        Hero hero = heroAction.GetComponent<Hero>();
        hero.TakeDamage(attack.Damage);

        EndTurn();
    }

    void EndTurn()
    {
        if (combatManager == null)
        {
            Debug.LogError("CombatManager introuvable");
            return;
        }
        hasFinished = true;
        combatManager.PlayerTurn();
    }

    public void SetNewHero(Action hero)
    {
        heroAction = hero;
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Skills_Structure", menuName = "Scriptable Objects/Skills_Structure")]
public class Skills_Structure : ScriptableObject
{
    public string SkillName;
    public Sprite Icon;
    [Header("Gameplay")]
    public float Damage;
    public float Critique; //Pour les position
    public string Condition;
    public string Effets; //Déséquilibre / Chute / Commotion | Hémorragie | Debuffs
    public string Description; //Info : Attaque de Flancs déséquilibre etc....
    public bool IsActive;
}

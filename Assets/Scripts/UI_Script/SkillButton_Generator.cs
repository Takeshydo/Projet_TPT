using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButtonGenerator : MonoBehaviour
{
    public GameObject buttonPrefab;   // Prefab du bouton
    public Transform buttonParent;    // Conteneur des boutons
    public Skille_Table skillTable;   // La database
    public Action HeroAction; //les Actions

    // Touche à afficher par ordre de génération
    private readonly string[] buttonKeys = { "&", "é", "\"" };

    public void GenerateSkillButtons()
    {

        void Start()
        {
            HeroAction = Action.GetComponent<Action>();
        }
        // Supprimer les anciens boutons
        foreach (Transform child in buttonParent)
            Destroy(child.gameObject);

        if (skillTable == null || skillTable.Skills == null)
        {
            Debug.LogWarning("SkillTable est vide ou non assignée !");
            return;
        }

        int keyIndex = 0; // index pour les touches

        // Générer un bouton par skill actif, max 3
        foreach (Skills_Structure skill in skillTable.Skills)
        {
            if (!skill.IsActive)
                continue;

            if (keyIndex >= buttonKeys.Length) // Limite à 3 boutons
                break;

            GameObject newButton = Instantiate(buttonPrefab, buttonParent);

            TMP_Text txt = newButton.GetComponentInChildren<TMP_Text>();
            if (txt != null){
                txt.text = $"{buttonKeys[keyIndex]} - {skill.SkillName}";
                HeroAction.equippedSkills.Add(skill);
            }
            Button btn = newButton.GetComponent<Button>();
            Skills_Structure capturedSkill = skill;
            btn.onClick.AddListener(() => HeroAction.AttackAction(capturedSkill));
            keyIndex++; // Passe à la touche suivante
        }
    }
}

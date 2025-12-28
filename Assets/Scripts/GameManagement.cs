using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance;
    public bool enteredFromBack = false;
    public GameObject SelectedPrefabs;
    public string CurrentenemyID;
    public HashSet<string> defeatedEnemies = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void enemy()
    {

    }
}




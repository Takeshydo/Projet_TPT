using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static GameManagement Instance;
    public bool enteredFromBack = false;

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

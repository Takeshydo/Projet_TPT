using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_Detection : MonoBehaviour
{

    private bool OntheBack = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          SceneManager.LoadScene("Arena");
        }
    }

}

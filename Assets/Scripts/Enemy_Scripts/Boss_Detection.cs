using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_Detection : MonoBehaviour
{
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

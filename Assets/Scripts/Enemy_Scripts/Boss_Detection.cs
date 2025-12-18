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
      Debug.Log("Assign prefab : " + gameObject.name);
      GameManagement.Instance.SelectedPrefabs = gameObject;
      SceneManager.LoadScene("Arena");
    }
  }


}

using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void OnButtonChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

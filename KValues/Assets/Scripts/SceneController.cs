using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SelectMode(string mode)
    {
        PlayerPrefs.DeleteKey("VALUES");
        PlayerPrefs.DeleteKey("PROCESS");
        PlayerPrefs.SetString("MODE", mode);
        Move("SelectCard");
    }
    public void Move(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

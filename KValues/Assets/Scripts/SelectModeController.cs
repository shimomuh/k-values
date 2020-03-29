using UnityEngine;
using UnityEngine.UI;

public class SelectModeController : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;

    void Awake()
    {
        if (PlayerPrefs.GetString("VALUES").Length != 0)
        {
            continueButton.SetActive(true);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeciceCardController : MonoBehaviour
{
    [SerializeField] private GameObject smallCard;
    [SerializeField] private GameObject gridLayout;
    [SerializeField] private Button button;
    [SerializeField] private Text text;
    private string[] values = new string[] {};
    private List<string> selectedValues = new List<string>() { };

    void Awake()
    {
        button.interactable = false;
        values = PlayerPrefs.GetString("VALUES").Split(',');
        for (var i = 0; i < values.Length; i++)
        {
            SmallCard card = Instantiate(smallCard).GetComponent<SmallCard>();
            card.transform.parent = gridLayout.transform;
            card.SetText(values[i]);
            card.ClickCallback = ClickCallback;
        }
        PlayerPrefs.DeleteKey("SHARE");
    }

    public void OnClick()
    {
        SceneManager.LoadScene("ResultShare");
        selectedValues.ForEach((v) => {
            if (PlayerPrefs.GetString("SHARE").Length == 0)
            {
                PlayerPrefs.SetString("SHARE", v);
            }
            else
            {
                PlayerPrefs.SetString("SHARE", $"{PlayerPrefs.GetString("SHARE")},{v}");
            }
        });
    }

    public void ClickCallback(string value, bool isSelected)
    {
        if (isSelected)
        {
            selectedValues.Add(value);
        } else {
            selectedValues.Remove(value);
        }

        if (selectedValues.Count == 0)
        {
            text.text = "あと3個選んでください";
            button.interactable = false;
            return;
        }
        if (selectedValues.Count == 1)
        {
            text.text = "あと2個選んでください";
            button.interactable = false;
            return;
        }
        if (selectedValues.Count == 2)
        {
            text.text = "あと1個選んでください";
            button.interactable = false;
            return;
        }
        if (selectedValues.Count > 3)
        {
            text.text = "減らしてください！";
            button.interactable = false;
            return;
        }
        text.text = "これらに決定する";
        button.interactable = true;
    }
}

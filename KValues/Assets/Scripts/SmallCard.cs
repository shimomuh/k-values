using System;
using UnityEngine;
using UnityEngine.UI;

public class SmallCard : MonoBehaviour
{
    [SerializeField] private Text text;
    private bool isSelected;
    private bool isReadonly;
    public Action<string, bool> ClickCallback;

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void SetReadonly(bool isReadonly)
    {
        this.isReadonly = isReadonly;
    }

    public void OnClick()
    {
        if (isReadonly) { return; }
        gameObject.GetComponent<Image>().color = isSelected ? new Color(0.184f, 0.451f, 0.945f) : new Color(1f, 0.725f, 0.153f);
        isSelected = !isSelected;
        ClickCallback(text.text, isSelected);
    }
}

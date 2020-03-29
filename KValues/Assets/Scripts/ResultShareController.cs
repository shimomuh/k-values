using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultShareController : MonoBehaviour
{
    [SerializeField] private GameObject smallCard;
    [SerializeField] private GameObject horizontalLayout;
    private string[] values = new string[] { };

    void Awake()
    {
        values = PlayerPrefs.GetString("SHARE").Split(',');
        for (var i = 0; i < values.Length; i++)
        {
            SmallCard card = Instantiate(smallCard).GetComponent<SmallCard>();
            card.transform.parent = horizontalLayout.transform;
            card.SetText(values[i]);
            card.SetReadonly(true);
        }
    }
}

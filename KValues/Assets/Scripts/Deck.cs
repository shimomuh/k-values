using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deck : MonoBehaviour
{
    [SerializeField] private GameObject deck;
    [SerializeField] private GameObject card;

    void Awake()
    {
        CardQueue cardQueue = new CardQueue();
        cardQueue.Initialize(deck, card);
    }
}

class CardQueue : MonoBehaviour
{
    private List<string> values = new List<string>() { "センスを大事にしたい", "努力を大事にしたい", "成長したい", "完璧でありたい", "ポジティブでありたい", "楽しいことをしたい" };
    private List<Card> cards = new List<Card>() { };
    private GameObject deck;
    private GameObject card;

    public void Initialize(GameObject deck, GameObject card)
    {
        this.deck = deck;
        this.card = card;
        if (PlayerPrefs.GetInt("PROCESS") > 0) {
            values.RemoveRange(0, values.Count - PlayerPrefs.GetInt("PROCESS") + 1);
        }
        if (values.Count == 0)
        {
            SceneManager.LoadScene("DecideCard");
            return;
        }
        SetTop();
        if (values.Count > 1)
        {
            SetMiddle();
        }
        if (values.Count > 2)
        {
            SetBottom();
        }
        if (values.Count > 3)
        {
            SetHide();
        }
        SetCallbacks();
    }

    public Card Set(Vector3 position, string value)
    {
        Card instance = Instantiate(card).GetComponent<Card>();
        instance.Initialize(deck, position, value);
        return instance;
    }
    private void SetTop()
    {
        Card card = Set(new Vector3(0, 0, 0), values[0]);
        card.SetDraggable(true);
        card.SetOrder(values.Count);
        cards.Add(card);
    }
    private void SetMiddle()
    {
        Card card = Set(new Vector3(20, 10, 0), values[1]);
        card.SetOrder(values.Count - 1);
        cards.Add(card);
    }
    private void SetBottom()
    {
        Card card = Set(new Vector3(40, 20, 0), values[2]);
        card.SetOrder(values.Count - 2);
        cards.Add(card);
    }
    private void SetHide()
    {
        if (values.Count <= 3) {
            return;
        }
        Card card = Set(new Vector3(40, 20, 0), values[3]);
        card.SetMovable(false);
        card.SetOrder(values.Count - 3);
        cards.Add(card);
    }
    private void SetCallbacks()
    {
        cards.ForEach((c) => {
            c.DraggedCallback = DraggedCallback;
            c.AutoMoveCallback = AutoMoveCallback;
            c.AutoMoveEndCallback = AutoMoveEndCallback;
        });
    }
    public void DraggedCallback(Card.Result result)
    {
        if (result == Card.Result.Yes) {
            string saveData = PlayerPrefs.GetString("VALUES");
            if (saveData.Length != 0)
            {
                saveData = $"{saveData},";
            }
            PlayerPrefs.SetString("VALUES", $"{saveData}{values[0]}");
        }

        PlayerPrefs.SetInt("PROCESS", values.Count);

        if (values.Count == 1) {
            SceneManager.LoadScene("DecideCard");
            return;
        }
        cards.RemoveAt(0);
        values.RemoveAt(0);
        SetHide();
        cards.ForEach((c) => c.SetMovable(true));
        cards[0].SetDraggable(true);
        if (cards.Count > 1)
        {
            cards[cards.Count - 1].SetMovable(false);
        }
        SetCallbacks();
    }
    public void AutoMoveCallback(float movement)
    {
        cards.ForEach((card) => card.AutoMove(movement));
    }
    public void AutoMoveEndCallback(float movement)
    {
        cards.ForEach((card) => card.AutoMoveEnd(movement));
    }
}
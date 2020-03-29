using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Text text;
    private bool isDraggable;
    private bool isAutoMove;
    private bool isVertical;
    private Vector3 originPosition;
    private Quaternion originRotation;
    public Action<Result> DraggedCallback;
    public Action<float> AutoMoveCallback;
    public Action<float> AutoMoveEndCallback;

    public static int THRESHOLD = 250;

    public enum Result
    {
        Yes,
        No,
        Neither
    };

    public void Initialize(GameObject parent, Vector3 position, string value)
    {
        SetParent(parent);
        SetPosition(position);
        SetText(value);
        SetDraggable(false);
        SetMovable(true);
        originPosition = transform.position;
        originRotation = transform.rotation;
    }

    public void SetDraggable(bool isDraggable)
    {
        this.isDraggable = isDraggable;
    }

    public void SetMovable(bool isAutoMove)
    {
        this.isAutoMove = isAutoMove;
    }

    public void SetOrder(int order)
    {
        GetComponent<Canvas>().sortingOrder = order;
    }

    private void SetText(string value)
    {
        text.text = value;
    }

    private void SetParent(GameObject parent)
    {
        transform.parent = parent.transform;
    }

    private void SetPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void AutoMove(float movement)
    {
        if (!isAutoMove)
        {
            return;
        }
        if (isDraggable)
        {
            return;
        }
        if (movement > THRESHOLD)
        {
            transform.position = new Vector3(originPosition.x - 20, originPosition.y - 10, 0);
            return;
        }
        transform.position = new Vector3(originPosition.x - 20 * movement / THRESHOLD, originPosition.y - 10 * movement / THRESHOLD, 0);
        return;
    }
    public void AutoMoveEnd(float movement)
    {
        if (!isAutoMove)
        {
            return;
        }
        if (isDraggable)
        {
            return;
        }
        transform.position = new Vector3(originPosition.x - 20 * movement / THRESHOLD, originPosition.y - 10 * movement / THRESHOLD, 0);
        return;
    }

    public void OnBeginDrag(PointerEventData data)
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
    }
    public void OnDrag(PointerEventData data)
    {
        float x = Mathf.Abs(data.position.x - originPosition.x);
        float y = Mathf.Abs(data.position.y - originPosition.y);
        float movement = x > y ? x : y;
        AutoMoveCallback(movement);

        if (!isDraggable)
        {
            return;
        }
        if (originPosition.y - data.position.y > 20)
        {
            isVertical = true;
        }
        else
        {
            isVertical = false;
        }

        if (isVertical)
        {
            transform.position = new Vector3(originPosition.x, data.position.y, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            return;
        }
        transform.position = new Vector3(data.position.x, originPosition.y - Mathf.Abs(originPosition.x - data.position.x) / 10, 0);
        transform.rotation = Quaternion.Euler(0, 0, (originPosition.x - data.position.x) / 45f);
    }
    
    public void OnEndDrag(PointerEventData data)
    {
        if (transform.position.x - originPosition.x > THRESHOLD)
        {
            AutoMoveEndCallback(THRESHOLD);
            DraggedCallback(Result.Yes);
            SetDraggable(false);
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        if (transform.position.x - originPosition.x < -THRESHOLD)
        {
            AutoMoveEndCallback(THRESHOLD);
            DraggedCallback(Result.No);
            SetDraggable(false);
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        if (originPosition.y - transform.position.y > THRESHOLD)
        {
            AutoMoveEndCallback(THRESHOLD);
            DraggedCallback(Result.Neither);
            SetDraggable(false);
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        AutoMoveEndCallback(0);
        transform.position = originPosition;
        transform.rotation = originRotation;
    }
}
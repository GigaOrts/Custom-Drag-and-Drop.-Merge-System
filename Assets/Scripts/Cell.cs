using UnityEngine;

public class Cell : MonoBehaviour
{
    private Item _item;
    
    public bool IsEmpty => _item == null;

    public void AcceptItem(Item newItem)
    {
        if (newItem.CurrentCell != null && newItem.CurrentCell != this)
            newItem.CurrentCell.Clear();

        _item = newItem;
        _item.transform.SetParent(transform, false);
        _item.RectTransform.anchoredPosition = Vector2.zero;
        _item.SetCell(this);
    }

    public void Clear()
    {
        _item = null;
    }
}
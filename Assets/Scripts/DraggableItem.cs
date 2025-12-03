using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Item))]
public class DraggableItem : MonoBehaviour
{
    private Item _item;

    private RectTransform _rect;
    private Transform _originalParent;
    private Vector3 _originalPosition;

    private Canvas _canvas;
    private Image _image;

    private bool _isDragging;

    void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _item = GetComponent<Item>();
    }

    private void Start()
    {
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnGrab()
    {
        _isDragging = true;

        _originalParent = transform.parent;
        _originalPosition = _rect.position;

        _item.Detach();

        transform.SetParent(_canvas.transform, true);
        _image.raycastTarget = false;
    }

    public void OnDrag(Vector3 mousePos)
    {
        if (!_isDragging) 
            return;
        
        _rect.position = mousePos;
    }

    public void OnRelease(Cell targetCell)
    {
        _isDragging = false;
        _image.raycastTarget = true;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        Item targetItem = null;

        foreach (var hit in results)
        {
            if (hit.gameObject == gameObject)
                continue;

            if (hit.gameObject.TryGetComponent(out Item foundItem))
            {
                targetItem = foundItem;
                break;
            }
        }

        if (targetItem != null)
        {
            TryMerge(targetItem);
            return;
        }

        if (targetCell != null && targetCell.IsEmpty)
        {
            targetCell.AcceptItem(_item);
            return;
        }

        transform.SetParent(_originalParent, true);
        _rect.position = _originalPosition;
        
        if (_originalParent.TryGetComponent(out Cell originalCell))
            originalCell.AcceptItem(_item);
    }

    private void TryMerge(Item target)
    {
        if (_item.CanMergeWith(target))
        {
            _item.MergeInto(target);
        }
        else
        {
            transform.SetParent(_originalParent, true);
            _rect.position = _originalPosition;
            if (_originalParent.TryGetComponent(out Cell originalCell))
                originalCell.AcceptItem(_item);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragDropManager : MonoBehaviour
{
    private DraggableItem currentDragged;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryGrabItem();
        else if (Input.GetMouseButton(0) && currentDragged != null)
            DragItem();
        else if (Input.GetMouseButtonUp(0) && currentDragged != null)
            DropItem();
    }
    // TODO: Try to declare a method for eventSystem Code duplication in TryGrabItem and DropItem
    void TryGrabItem()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var hit in results)
        {
            if (hit.gameObject.TryGetComponent(out DraggableItem item))
            {
                currentDragged = item;
                item.OnGrab();
                break;
            }
        }
    }

    void DragItem()
    {
        currentDragged.OnDrag(Input.mousePosition);
    }

    void DropItem()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        Cell targetCell = null;

        foreach (var hit in results)
        {
            if (hit.gameObject.TryGetComponent(out Cell cell))
            {
                targetCell = cell;
                break;
            }
        }

        currentDragged.OnRelease(targetCell);
        currentDragged = null;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CardDarg : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static Vector2 originalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = Input.mousePosition;
        transform.position = currentPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = originalPosition;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

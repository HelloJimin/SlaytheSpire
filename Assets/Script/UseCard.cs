using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UseCard : MonoBehaviour , IPointerClickHandler , IBeginDragHandler , IDragHandler , IPointerEnterHandler , IPointerExitHandler
{
    public static Vector2 originalPosition;
    private GameObject target;
    bool isGrab;

    #region 드래그부분
    public void OnBeginDrag(PointerEventData eventData)
    {
        isGrab = true;
        originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isGrab = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isGrab)
        {
            isGrab = true;
            originalPosition = transform.position;
        }
        else
        {
            isGrab = false;

            CastRay();

            if(!GetComponent<Card>().Use(target))
            {
                transform.position = originalPosition;
            }
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter);
        transform.localScale += new Vector3(1.0f, 1.0f);
       // transform.localPosition = new Vector3(transform.localPosition.x, -110, transform.localPosition.z);

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale -= new Vector3(1.0f, 1.0f);

    }

    #endregion

    void CastRay()
    {
        target = null;
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            target = hit.collider.gameObject;
        }
    }


    private void Update()
    {
        if (!isGrab) return;

        transform.position = Input.mousePosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{

    //카드와 마우스의 상호작용을 하는 부분입니다

    public static Vector2 originalPosition;
    private GameObject target;
    public Character playerData;
    bool isGrab;

    #region 드래그부분
    public void OnBeginDrag(PointerEventData eventData)
    {
        isGrab = true;
        originalPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isGrab)
        {
            isGrab = false;

            CastRay();

            if (transform.position.y < 350)
            {
                return;
            }
            if (!GetComponent<Card>().Use(target))
            {
                transform.position = originalPosition;
            }
        }
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

            if (!GetComponent<Card>().Use(target))
            {
                transform.position = originalPosition;
            }
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //transform.localScale += new Vector3(1.0f, 1.0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //transform.localScale -= new Vector3(1.0f, 1.0f);
    }

    #endregion

    private void Start()
    {
        playerData = FindObjectOfType<Player>();
    }

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

    private void FixedUpdate()
    {
        if (!isGrab) return;

        transform.position = Input.mousePosition;

        if (transform.position.y > 350)
        {
            if (GetComponent<Card>().card.cost > GameManager.instance.currentCost)
            {
                isGrab = false;
                transform.position = originalPosition;
                Debug.Log("코스트부족");
            }
        }
    }
}

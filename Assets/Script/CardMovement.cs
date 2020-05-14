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

    Card card;
    #region 드래그부분
    public void OnBeginDrag(PointerEventData eventData)
    {
        switch (UIManager.instance.choice)
        {
            case ChoiceMode.Grab:
                break;
            case ChoiceMode.Upgrade:
                if (UIManager.instance.choicePanel.transform.childCount < UIManager.instance.choiceSize)
                {
                    transform.SetParent(UIManager.instance.choicePanel.transform);
                }
                else
                {
                    UIManager.instance.choicePanel.transform.GetChild(0).SetParent(GameManager.instance.myHand.transform);
                    transform.SetParent(UIManager.instance.choicePanel.transform);
                }
                return;
            case ChoiceMode.Choice:
              Card newca=  GameManager.instance.AddCardToDeck(card.GetType().Name);
                GameManager.instance.inventory.Add(newca);
                return;
        }
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

            UseCard();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (UIManager.instance.choice)
        {
            case ChoiceMode.Grab:
                break;
            case ChoiceMode.Upgrade:
                if (UIManager.instance.choicePanel.transform.childCount < UIManager.instance.choiceSize)
                {
                    transform.SetParent(UIManager.instance.choicePanel.transform);
                }
                else
                {
                    UIManager.instance.choicePanel.transform.GetChild(0).SetParent(GameManager.instance.myHand.transform);
                    transform.SetParent(UIManager.instance.choicePanel.transform);
                }
                return;
            case ChoiceMode.Choice:
                Card newca = GameManager.instance.AddCardToDeck(card.GetType().Name);
                GameManager.instance.inventory.Add(newca);
                return;
        }

        if (!isGrab)
        {
            isGrab = true;
            originalPosition = transform.position;
        }
        else
        {
            isGrab = false;

            CastRay();

            UseCard();
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
        card = GetComponent<Card>();
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

    private void UseCard()
    {
        if (target == null)
        {
            if (card.card.target == CardTarget.All)
            {
                if (card.CostCheck())
                {
                    Debug.Log(card.card.name + "사용!!");
                    card.GoCenter(GameManager.instance.player);
                }
                else
                {
                    Debug.Log("코스트가 부족합니다!!");
                    transform.position = originalPosition;
                }
            }
            else
            {
                Debug.Log("대상이 아닙니다!!");
                transform.position = originalPosition;
            }
            return;
        }

        if (card.TargetCheck(target.GetComponent<Character>()))
        {
            if (card.CostCheck())
            {
                Debug.Log(card.card.name+ "사용!!");
                card.GoCenter(target.GetComponent<Character>());
            }
            else
            {
                Debug.Log("코스트가 부족합니다!!");
                transform.position = originalPosition;
            }
        }
        else
        {
            Debug.Log("대상이 아닙니다!!");
            transform.position = originalPosition;
        }
    }
}

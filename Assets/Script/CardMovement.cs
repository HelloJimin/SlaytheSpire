﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{
    //카드와 마우스의 상호작용을 하는 부분입니다

    public Vector3 orignalScale;
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
            case ChoiceMode.None:
                return;
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
                GameManager.instance.myInventoryList.Add(card.GetType().Name);
                UIManager.instance.SucceesChoice();
                return;
            case ChoiceMode.RestUpgrade:
                UIManager.instance.RestCardGoToUpgradePanel(card);
                return;
            case ChoiceMode.Shop:
                if (playerData.data.money > card.Price)
                {
                    playerData.data.money -= card.Price;
                    card.gameObject.SetActive(false);
                    GameManager.instance.myInventoryList.Add(card.GetType().Name);
                    UIManager.instance.shopHand.GoToOrignalPosition();
                    UIManager.instance.SettingUI();
                    SoundManager.instance.PlaySound("BuyCard");
                    return;
                }
                else
                {
                    return;
                }
        }
        isGrab = true;
        originalPosition = transform.position;
        SoundManager.instance.PlaySound("CardClick");
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
            case ChoiceMode.None:
                return;
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
                GameManager.instance.myInventoryList.Add(card.GetType().Name);
                UIManager.instance.SucceesChoice();
                return;
            case ChoiceMode.RestUpgrade:
                UIManager.instance.RestCardGoToUpgradePanel(card);
                return;
            case ChoiceMode.Shop:
                if (playerData.data.money > card.Price)
                {
                    playerData.data.money -= card.Price;
                    card.gameObject.SetActive(false);
                    GameManager.instance.myInventoryList.Add(card.GetType().Name);
                    UIManager.instance.shopHand.GoToOrignalPosition();
                    UIManager.instance.SettingUI();
                    SoundManager.instance.PlaySound("BuyCard");
                    return;
                }
                else
                {
                    return;
                }
        }
        if (!isGrab)
        {
            isGrab = true;
            originalPosition = transform.position;
            SoundManager.instance.PlaySound("CardClick");
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
        switch (UIManager.instance.choice)
        {
            case ChoiceMode.None:
                break;
            case ChoiceMode.Grab:
                break;
            case ChoiceMode.Upgrade:
                SoundManager.instance.PlaySound("CardHover");
                break;
            case ChoiceMode.Choice:
                SoundManager.instance.PlaySound("CardHover");
                break;
            case ChoiceMode.RestUpgrade:
                break;
            case ChoiceMode.Shop:
                UIManager.instance.shopHand.GoToMousePosition(card.transform.position + new Vector3(0,400,0));
                //orignalScale = transform.localScale;
                //transform.localScale += new Vector3(1.0f, 1.0f);
                break;
        }
     
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (UIManager.instance.choice)
        {
            case ChoiceMode.None:
                break;
            case ChoiceMode.Grab:
                break;
            case ChoiceMode.Upgrade:
                break;
            case ChoiceMode.Choice:
                break;
            case ChoiceMode.RestUpgrade:
                break;
            case ChoiceMode.Shop:
                UIManager.instance.shopHand.GoToOrignalPosition();
                // transform.localScale = orignalScale;
                break;
        }
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

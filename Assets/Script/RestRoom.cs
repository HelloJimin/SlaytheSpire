using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class RestRoom : MonoBehaviour
{
    public int actionPoint;
    public GameObject playerAnime;
    Vector3 original;

    Button[] buttons;

    public GameObject myAllCardList;

    void Start()
    {
        
    }

    public void OnEnable()
    {
        if (buttons == null)
        {
             buttons = GetComponentsInChildren<Button>();
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.SetActive(true);
            }
        }

        actionPoint = 1;
        PlayerAnimeMove();

        ButtonColorChange(new Color(255, 255, 255, 255));
    }

    public void OnDisable()
    {
        playerAnime.transform.localPosition = original;
        myAllCardList.SetActive(false);
    }

    public void Rest()
    {
        if (actionPoint == 0)
        {
            return;
        }

        int heal = (int)(GameManager.instance.player.data.maxHP * 0.3f);

        if (GameManager.instance.player.data.currentHP + heal > GameManager.instance.player.data.maxHP)
        {
            GameManager.instance.player.data.currentHP = GameManager.instance.player.data.maxHP;
        }
        else
        {
           GameManager.instance.player.data.currentHP += heal;
        }

        UIManager.instance.SettingUI();
        UIManager.instance.proceedButton.SetActive(true);
        actionPoint--;
        ButtonColorChange(new Color(255, 255, 255,0.5f));
    }

    public void Upgrade()
    {
        if (actionPoint == 0)
        {
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }

        myAllCardList.SetActive(true);

        UIManager.instance.choice = ChoiceMode.RestUpgrade;
        GameObject inventoryCards = myAllCardList.transform.Find("AllCards").gameObject;
        GameManager.instance.SettingMyDeck(inventoryCards);

        for (int i = 0; i < inventoryCards.transform.childCount; i++)
        {
            inventoryCards.transform.GetChild(i).gameObject.SetActive(true);
        }
        //이부분 마이올카드리스트 스크립트로 옮기자..
       // UIManager.instance.choice=ChoiceMode.
        UIManager.instance.SettingUI();
        UIManager.instance.proceedButton.SetActive(false);
        actionPoint--;
        ButtonColorChange(new Color(255, 255, 255, 0.5f));
    }

    void PlayerAnimeMove()
    {
        original = playerAnime.transform.localPosition;

        playerAnime.transform.DOLocalMove(new Vector3(-250, -24, 0), 0.5f);
    }

    void ButtonColorChange(Color color)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Image>().color = color;
        }
    }
}

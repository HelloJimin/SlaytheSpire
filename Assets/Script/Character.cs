using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;

public class Character : MonoBehaviour
{
    public CharacterData data;
    public bool isPlayer;
    public int vulnerable;
    public int weak;
    public int frail;

    public delegate void MyTurnEnd();
    public event MyTurnEnd myTurnEnd;
    public delegate void MyTurnStart();
    public event MyTurnStart myTurnStart;

    public SkeletonAnimation anime;

    public Text HPText;

    public virtual void Start()
    {
        HPText = transform.Find("Canvas/MyHP/HPText").GetComponent<Text>();
        anime = GetComponent<SkeletonAnimation>();
        myTurnEnd += CoolDown;
        SettingHPUI();
    }

    public virtual void Hit(float damage)
    {
        damage = DefenseDamageCheck(damage);

        data.currentHP -= (int)damage;
        SettingHPUI();
        //ShieldBreak();
        UIManager.instance.CameraShake();
    }

    public int DefenseDamageCheck(float damage)
    {
        float Damage = damage;

        if (vulnerable > 0) Damage *= 1.5f;

        if (data.shield>0)
        {
            float temp = Damage;
            Damage -= data.shield;
            data.shield -= (int)temp;
            ShieldBreak();
            if (data.shield <= 0)
            {
                data.shield = 0;
                SettingShieldUI(false);
            }
            else
            {
                SettingShieldUI(true);
            }
        }

        if (Damage < 0) Damage = 0;

        return (int)Damage;
    }

    public void GetShield(int sheild)
    {
        data.shield += sheild + data.dexterity;
        if (frail > 0) data.shield -= (int)(data.shield * 0.25f);

        SettingShieldUI(true);
    }

    public void ShieldBreak(int damage = 0)
    {
        if (data.shield > 0) return;

        SettingShieldUI(false);
    }

    void SettingShieldUI(bool isShield)
    {
        if (isShield)
        {
            transform.Find("Canvas").transform.Find("MyHP").GetComponent<Outline>().enabled = true;
            transform.Find("Canvas/MyHP/HPBar").GetComponent<Image>().color = Color.blue;
            transform.Find("Canvas/MyHP/SheildImage").gameObject.SetActive(true);
            transform.Find("Canvas/MyHP/SheildImage").GetComponentInChildren<Text>().text = data.shield.ToString();
        }
        else
        {
            transform.Find("Canvas/MyHP/SheildImage").gameObject.SetActive(false);
            transform.Find("Canvas/MyHP/HPBar").GetComponent<Image>().color = Color.red;
            transform.Find("Canvas").transform.Find("MyHP").GetComponent<Outline>().enabled = false;
        }
    }

    public void MyEndPhase()
    {
        myTurnEnd.Invoke();
    }

    public void YourEndPhase()
    {
        data.shield = 0;
        ShieldBreak();
    }

    public void MyStartPhase()
    {
        myTurnStart.Invoke();
    }

    void CoolDown()
    {
        if (frail > 0) frail--;
        if (vulnerable > 0) vulnerable--;
        if (weak > 0) weak--;
    }

    public virtual void Animation(string aniName, bool isLeft)
    {
        float position;

        if (isLeft)
        {
            position = -0.5f;
        }
        else
        {
            position = 1;
        }

        if (aniName == "Atk")
        {
            transform.DOMove(new Vector3(transform.position.x + position, transform.position.y, transform.position.z), 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Flash);
        }
        else
        {

        }
    }

    public void SettingHPUI()
    {
        HPText.text = data.currentHP + "/" + data.maxHP;
        HPText.transform.parent.Find("HPBar").GetComponent<Image>().fillAmount = (float)data.currentHP / (float)data.maxHP;
    }
}

[System.Serializable]
public struct CharacterData
{
    public string name;
    public int currentHP;
    public int maxHP;

    public int shield;
    public int power;
    public int dexterity;
    public int money;
}
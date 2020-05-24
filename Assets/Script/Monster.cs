using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class Monster : Character
{
    public Image intentImage;

    public float damage;
    public Character player;
    public bool isAttack;
    Text damageText;

    public override int Power { get => base.Power; set { base.Power = value; SettingDamageUI(); } }

    public void Init(int maxHp, int money)
    {
        DataInit();
        ResetStatusUI();
        data.maxHP = maxHp;
        data.currentHP = data.maxHP;
        data.money = money;
        data.power = 0;
       // SettingHPUI();
        transform.Find("Canvas").transform.Find("MyHP").transform.Find("HPBar").GetComponent<Image>().fillAmount = (float)data.currentHP / (float)data.maxHP;
        transform.Find("Canvas").GetComponentInChildren<Text>().text = data.currentHP + "/" + data.maxHP;
    }

    public void Awake()
    {
        intentImage = transform.Find("Canvas").transform.Find("Intent").GetComponent<Image>();
        damageText = intentImage.transform.Find("Damage").GetComponent<Text>();
        SettingAnimation();
       // ActionCheck();

        myTurnStart += Action;
        myTurnEnd += ActionCheck;
        myTurnEnd += SettingDamageUI;
    }

    public override void Start()
    {
        base.Start();
        player = GameManager.instance.player;
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
        player.Animation("Atk",false);
        if (data.currentHP<=0)
        {
            Dead();
        }
    }

    public virtual void Dead()
    {
        GameManager.instance.monsters.Remove(this);
        GameManager.instance.currentRoomMoney += data.money;
        ObjectPoolManager.instance.ReturnMonster(this);
        UIManager.instance.SettingUI();
        GameManager.instance.BattleEnd();
    }

    public virtual void Action()
    {

    }
    public virtual void Attack()
    {

    }

    IEnumerator returnAnime(string name)
    {
        yield return new WaitForSeconds(0.1f);
        spain.AnimationState.SetAnimation(0, name, true);
    }

    public void FindIntentImage(string name)
    {
        intentImage.gameObject.SetActive(true);

        intentImage.sprite = Resources.Load<Sprite>("Sprite/Intent/" + name) as Sprite;
        SettingDamageUI();
    }

    public void SettingAnimation()
    {
        spain = GetComponent<SkeletonAnimation>();
        spain.skeletonDataAsset = Resources.Load<SkeletonDataAsset>("Sprite/Monster/"+GetType().Name+"/"+GetType().Name+"Data") as SkeletonDataAsset;

        spain.loop = true;
        spain.AnimationName = "";
        spain.Awake();
    }

    public virtual void ActionCheck()
    {
        if (intentImage.sprite.ToString() == "attack1")
        {
            intentImage.GetComponent<Text>().enabled = true;
            intentImage.GetComponent<Text>().text = AttackDamageCheck().ToString();
        }
    }

    public virtual int AttackDamageCheck()
    {
       float realDamage;
        
       if (weak > 0)
       {
           realDamage = damage - (damage * 0.25f) + data.power;
           if (realDamage < 0) realDamage = 0;
       }
       else
       {
           realDamage = damage + data.power;
       }

       if (player != null && player.Vulnerable > 0)
       {
           realDamage *= 1.5f;
       }
        return (int)realDamage;
    }

    public void SettingDamageUI()
    {
        if (isAttack)
        {
            damageText.enabled = true;
            damageText.text = AttackDamageCheck().ToString();
        }
        else
        {
            damageText.enabled = false;
        }
    }

    public void ResetStatusUI()
    {
        while (transform.Find("Canvas/StatusPanel").childCount != 0)
        {
            ObjectPoolManager.instance.ReturnStatusUI(transform.Find("Canvas/StatusPanel").GetChild(0).GetComponent<StatusUI>());
        }
        statusUIs.Clear();
    }
}
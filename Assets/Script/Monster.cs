using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class Monster : Character
{
    public Image intentImage;
    public SkeletonAnimation spain;
    public float damage;
    public Character player;

    public void Init(int maxHp, int money)
    {
        data.maxHP = maxHp;
        data.money = money;
        data.currentHP = data.maxHP;

        transform.Find("Canvas").transform.Find("MyHP").transform.Find("HPBar").GetComponent<Image>().fillAmount = (float)data.currentHP / (float)data.maxHP;
        transform.Find("Canvas").GetComponentInChildren<Text>().text = data.currentHP + "/" + data.maxHP;
    }

    public void Awake()
    {
        intentImage = transform.Find("Canvas").transform.Find("Intent").GetComponent<Image>();
        SettingAnimation();
        ActionCheck();
        myTurnStart += Action;
    }

    private void Start()
    {
        player = GameManager.instance.player;
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
        if (data.currentHP<=0)
        {
            GameManager.instance.player.data.money += data.money;
            ObjectPoolManager.instance.ReturnMonster(this);
            UIManager.instance.SettingUI();
        }
    }

    public virtual void Action()
    {

    }
    public virtual void Attack()
    {

    }

    public void FindIntentImage(string name)
    {
        intentImage.sprite = Resources.Load<Sprite>("Sprite/Intent/" + name) as Sprite;
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
        return (int)realDamage;
    }
}
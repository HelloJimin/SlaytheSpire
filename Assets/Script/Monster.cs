using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class Monster : Character
{
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
        SettingAnimation();
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

    public virtual void Attack()
    {

    }


    void SettingAnimation()
    {
        SkeletonAnimation spain;

        spain = GetComponent<SkeletonAnimation>();
        spain.skeletonDataAsset = Resources.Load<SkeletonDataAsset>("Sprite/Monster/"+GetType().Name+"/"+GetType().Name+"Data") as SkeletonDataAsset;

        spain.loop = true;
        spain.AnimationName = "animation";
        spain.Awake();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeL : Slime
{
    private void OnEnable()
    {
        spain.Initialize(true);
        int hp = Random.Range(66, 70);
        int money = Random.Range(30, 40);
        action = CurrentAction.Debuff;
        Init(hp, money);
        AnimeChangeStart("Idle", true);
        ActionCheck();
    }

    public override void ActionCheck()
    {
        if (action != CurrentAction.Split)
        {
            int[] temp = { 0, 2, 3 };
            action = (CurrentAction)temp[Random.Range(0, temp.Length)];
        }

        switch (action)
        {
            case CurrentAction.Debuff:
                isAttack = false;
                FindIntentImage("debuff1");
                break;
            case CurrentAction.Attack:
                damage = 16;
                isAttack = true;
                FindIntentImage("attack3");
                break;
            case CurrentAction.AttackDebuff:
                damage = 11;
                isAttack = true;
                FindIntentImage("attackDebuff");
                break;
            case CurrentAction.Split:
                isAttack = false;
                FindIntentImage("unknown");
                break;
        }
    }

    public override void Action()
    {
        switch (action)
        {
            case CurrentAction.Debuff:
                Debuff();
                break;
            case CurrentAction.Attack:
                Attack();
                break;
            case CurrentAction.AttackDebuff:
                AttackDebuff();
                break;
            case CurrentAction.Split:
                Split("SlimeM", 2, data.currentHP, 0);
                break;
        }
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
        AnimeOneShotStart("damage");
    }

    void AttackDebuff()
    {
        base.Attack();

        for (int i = 0; i < 2; i++)
        {
            Card newcard = ObjectPoolManager.instance.GetCard("Wound");
            newcard.transform.SetParent(GameManager.instance.myDeck.transform);
        }
    }

    public override void Debuff()
    {
        player.Weak+=2;
    }
}

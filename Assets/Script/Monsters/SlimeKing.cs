using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing : Slime
{
    private void OnEnable()
    {
        spain.Initialize(true);
        int hp = 140;
        int money = 100;
        action = CurrentAction.Debuff;
        Init(hp, money);
        AnimeChangeStart("idle", true);
        ActionCheck();
        GameManager.instance.isEliteRoom = true;

    }

    public override void ActionCheck()
    {
        switch (action)
        {
            case CurrentAction.Debuff:
                isAttack = false;
                FindIntentImage("debuff2");
                break;
            case CurrentAction.Waiting:
                isAttack = false;
                FindIntentImage("unknown");
                break;
            case CurrentAction.Attack:
                damage = 35;
                isAttack = true;
                FindIntentImage("attack4");
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
                action = CurrentAction.Waiting;
                SoundManager.instance.PlaySound("SlimeBoss1");
                break;
            case CurrentAction.Waiting:
                action = CurrentAction.Attack;
                break;
            case CurrentAction.Attack:
                Attack();
                action = CurrentAction.Debuff;
                SoundManager.instance.PlaySound("SlimeBoss2");
                break;
            case CurrentAction.Split:
                Split("SlimeL",2,data.currentHP,0);
                break;
        }
    }

    public override void Debuff()
    {
        ObjectPoolManager.instance.GetDebuffCard("Slimed");
    }

    public override void Dead()
    {
        base.Dead();
        GameManager.instance.isClear = true;
    }
    public override void Split(string monsterName, int cnt, int hp, int money)
    {
        base.Split(monsterName, cnt, hp, money);
        GameManager.instance.isClear = true;
    }
}

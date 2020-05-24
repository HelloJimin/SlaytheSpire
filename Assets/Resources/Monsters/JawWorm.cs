using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JawWorm : Monster
{
    enum CurrentAction
    {
        Attack,
        AttackDefend,
        DefendBuff
    }
    CurrentAction action;

    private void OnEnable()
    {
        spain.Initialize(true);
        int hp = Random.Range(41, 45);
        int money = Random.Range(41, 45);
        Init(hp, money);
        AnimeChangeStart("idle", true);
        ActionCheck();
    }

    public override void ActionCheck()
    {
        action = (CurrentAction)Random.Range(0, 3);

        switch (action)
        {
            case CurrentAction.Attack:
                isAttack = true;
                damage = 11;
                FindIntentImage("attack2");
                break;
            case CurrentAction.AttackDefend:
                isAttack = true;
                damage = 7;
                FindIntentImage("attackDefend");
                break;
            case CurrentAction.DefendBuff:
                isAttack = false;
                FindIntentImage("defendBuff");
                break;
        }
    }

    public override void Action()
    {
        switch (action)
        {
            case CurrentAction.Attack:
                Attack();
                AnimeOneShotStart("chomp");
                break;
            case CurrentAction.AttackDefend:
                AttackDefend();
                break;
            case CurrentAction.DefendBuff:
                DefendBuff();
                AnimeOneShotStart("tailslam");
                break;
        }
    }

    public override void Attack()
    {
        player.Hit(AttackDamageCheck());
        Animation("Atk", true);
    }
    void AttackDefend()
    {
        player.Hit(AttackDamageCheck());
        Animation("Atk", true);
        GetShield(5);
    }
    void DefendBuff()
    {
        GetShield(6);
        Power += 3;
    }
}

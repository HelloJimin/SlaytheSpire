using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lagavulin : Monster
{
    int isSiphon;
    int sleepCnt;
    enum Status
    {
        Sleep,
        Stun,
        WakeUp
    }
    Status status;

    private void OnEnable()
    {
        spain.Initialize(true);
        int hp = Random.Range(110, 115);
        int money = Random.Range(30, 70);
        isAttack = false;
        isSiphon = 0;
        sleepCnt = 0;
        status = Status.Sleep;
        Init(hp, money);
        data.shield = 8;
        SettingShieldUI(true);
        // GetShield(8);
        myTurnEnd -= Sleep;
        myTurnEnd += Sleep;
        AnimeChangeStart("Idle_1",true);
        PropertySet("금속화", Resources.LoadAll<Sprite>("Sprite/powers")[150], 8, "턴 종료시 8의 방어도를 얻습니다.");
        ActionCheck();
        GameManager.instance.isEliteRoom = true;

    }

    public override void ActionCheck()
    {
        switch (status)
        {
            case Status.Sleep:
                FindIntentImage("sleep");
                return;
            case Status.Stun:
                myTurnEnd -= Sleep;

                FindIntentImage("stun");

                AnimeChangeStart("Coming_out", false);
                AnimeChangeStartForDelay("Idle_2", true, 0f);
                return;
            case Status.WakeUp:
                isSiphon++;

                if (isSiphon % 3 == 0)
                {
                    isAttack = false;
                    FindIntentImage("debuff2");
                }
                else
                {
                    isAttack = true;
                    damage = 18;
                    FindIntentImage("attack3");
                }
                break;
        }
    }

    public override void Action()
    {
        switch (status)
        {
            case Status.Sleep:
                sleepCnt++;
                if (sleepCnt == 3)
                {
                    status = Status.WakeUp;
                    myTurnEnd -= Sleep;
                    AnimeChangeStart("Coming_out", false);
                    AnimeChangeStartForDelay("Idle_2", true, 0f);
                }
                return;
            case Status.Stun:
                status = Status.WakeUp;
                return;
            case Status.WakeUp:
                if (isSiphon % 3 == 0)
                {
                    SiphonSoul();
                }
                else
                {
                    Attack();
                }

                intentImage.gameObject.SetActive(false);
                break;
        }
    }

    public override void Attack()
    {
        player.Hit(AttackDamageCheck());
        AnimeOneShotStart("Attack");
    }

    void SiphonSoul()
    {
        player.Power -= 1;
        player.Dexterity -= 1;
        AnimeOneShotStart("Debuff");
    }

    void Sleep()
    {
        GetShield(8); 
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);

        if (status == Status.Sleep)
        {
            if (data.currentHP< data.maxHP)
            {
                Debug.Log("체력" + data.currentHP);
                status = Status.Stun;
                RemoveStatusUI("금속화");
                ActionCheck();
            }
        }

        if (status == Status.WakeUp)
        {
            AnimeOneShotStart("Hit");
        }
    }
    public override void Dead()
    {
        base.Dead();
        GameManager.instance.eliteKill++;
    }
}

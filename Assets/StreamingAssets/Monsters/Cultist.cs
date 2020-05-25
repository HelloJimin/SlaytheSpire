using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cultist : Monster
{
    bool isFirstTurn;

    bool IsFirstTurn { get { return isFirstTurn; } set { isFirstTurn = value; PropertySet("의식", Resources.LoadAll<Sprite>("Sprite/powers")[6], 3, "매 턴 힘이 3 증가합니다."); } }

    private void OnEnable()
    {
        spain.Initialize(true);
        int hp = Random.Range(49, 55);
        int money = Random.Range(10, 30);
        Init(hp, money);

        AnimeChangeStart("rally", true);
        damage = 6;
        isFirstTurn = true;
        myTurnEnd -= PowerUP;
        isAttack = false;
        ActionCheck();
    }

    public override void ActionCheck()
    {
        FindIntentImage("buff");
        myTurnEnd -= ActionCheck;
    }

    public override void Action()
    {
        if (IsFirstTurn)
        {
            myTurnEnd += Ritual;
            isAttack = true;
            SettingDamageUI();
            IsFirstTurn = false;
            FindIntentImage("attack2");
            AnimeOneShotStart("waving");
        }
        else
        {
            Attack();
        }

    }

    public override void Attack()
    {
        player.Hit(AttackDamageCheck());
        Animation("Atk", true);
    }

    void PowerUP()
    {
        data.power += 3;
        Debug.Log("지금파워:" + data.power);
    }
    void Ritual()
    {
        myTurnEnd += PowerUP;
        myTurnEnd -= SettingDamageUI;
        myTurnEnd += SettingDamageUI;
        myTurnEnd -= Ritual;
    }
}

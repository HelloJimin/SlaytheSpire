using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeM : Slime
{
    private void OnEnable()
    {
        spain.Initialize(true);
        int hp = Random.Range(29, 33);
        int money = Random.Range(10, 20);
        action = CurrentAction.Debuff;
        Init(hp, money);
        AnimeChangeStart("idle", true);
        ActionCheck();
    }

    public override void ActionCheck()
    {
        int[] temp = { 0, 2, 3 };
        action = (CurrentAction)temp[Random.Range(0, temp.Length)];

        switch (action)
        {
            case CurrentAction.Debuff:
                isAttack = false;
                FindIntentImage("debuff1");
                break;
            case CurrentAction.Attack:
                damage = 10;
                isAttack = true;
                FindIntentImage("attack1");
                break;
            case CurrentAction.AttackDebuff:
                damage = 7;
                isAttack = true;
                FindIntentImage("attackDebuff");
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
        damage = DefenseDamageCheck(damage);

        data.currentHP -= (int)damage;
        SettingHPUI();
        UIManager.instance.CameraShake();
        player.Animation("Atk", false);
        AnimeOneShotStart("damage");
        SoundManager.instance.PlaySound("FastAtk1");
        if (data.currentHP <= 0)
        {
            Dead();
        }
    }

    void AttackDebuff()
    {
        base.Attack();

        ObjectPoolManager.instance.GetDebuffCard("Slimed");
    }

    public override void Debuff()
    {
        player.Weak+=2;
    }
}

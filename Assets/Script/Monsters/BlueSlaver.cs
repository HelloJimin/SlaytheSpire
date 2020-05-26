using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueSlaver : Monster
{
    bool isStab;

    private void OnEnable()
    {
        int hp = Random.Range(47, 51);
        int money = Random.Range(47, 51);
        Init(hp, money);
        isAttack = true;
        spain.AnimationName = "idle";
        ActionCheck();
    }

    public override void ActionCheck()
    {
        isStab = Random.value > 0.3f;

        if (isStab)
        {
            damage = 12;
            FindIntentImage("attack2");
        }
        else
        {
            damage = 7;
            FindIntentImage("attackDebuff");
        }
    }

    public override void Action()
    {
        if (isStab)
        {
            Attack();
        }
        else
        {
            Rake();
        }
    }

    public override void Attack()
    {
        player.Hit(AttackDamageCheck());
        Animation("Atk", true);
        SoundManager.instance.PlaySound("SlaverBlue");
    }

    void Rake()
    {
        player.Weak += 2;
        player.Hit(AttackDamageCheck());
        Animation("Atk", true);
    }
}

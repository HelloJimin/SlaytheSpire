using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LouseRed : Monster
{
    bool isCurlUp;
    bool IsCurlUp
    {
        get { return isCurlUp; }
        set
        {
            isCurlUp = value;
            if (isCurlUp)
            {
                PropertySet("isCurlUp", Resources.LoadAll<Sprite>("Sprite/powers")[53], 6, "공격 피해를 받으면 6의 방어도를 얻습니다.");
            }
            else
            {
                RemoveStatusUI("isCurlUp");
            }
        }
    }

    private void OnEnable()
    {
        spain.Initialize(true);
        int hp = Random.Range(10, 15);
        int money = Random.Range(10, 15);
        Init(hp, money);
        AnimeChangeStart("idle", true);
        damage = 6;
        IsCurlUp = true;
        ActionCheck();
    }

    public override void ActionCheck()
    {
        isAttack = Random.value > 0.3f;

        if (isAttack)
        {
            FindIntentImage("attack1");
        }
        else
        {
            FindIntentImage("buff");
        }
    }

    public override void Action()
    {
        if (isAttack)
        {
            Attack();
        }
        else
        {
            PowerUp();
        }
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);

        if (IsCurlUp)
        {
            IsCurlUp = false;
            AnimeChangeStart("transitiontoclosed", false);
            AnimeChangeStartForDelay("idle closed", false,0.3f);
            GetShield(6);
        }
    }

    void PowerUp()
    {
        AnimeOneShotStart("rear");
        Power += 3;
    }

    public override void Attack()
    {
        if (spain.AnimationName == "idle closed")
        {
            AnimeChangeStart("transitiontoopened", false);
            AnimeChangeStartForDelay("idle", true,0.1f);
        }
        damage = Random.Range(6, 8);
        player.Hit(AttackDamageCheck());
        Animation("Atk", true);
    }
}

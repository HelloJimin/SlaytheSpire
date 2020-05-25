using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class Player : Character
{
    public struct CardUseCnt
    {
        public int attack;
        public int skill;
        public int power;
    }

    public List<string> inventoryList;
    public List<string> artifactList;

    public GameObject hitPanel;

    public override int Vulnerable { get => base.Vulnerable; set { base.Vulnerable = value; GameManager.instance.MonstersDamageUIUpdate(); } }
    #region 이벤트

    public delegate void BattleEnded();
    public event BattleEnded battleEnded;
    public delegate void BattleStarted();
    public event BattleStarted battleStart;

    public delegate void AttackUseEvent();
    public event AttackUseEvent attackUseEvent;
    public delegate void SkillUseEvent();
    public event SkillUseEvent skillUseEvent;
    public delegate void PowerUseEvent();
    public event PowerUseEvent powerUseEvent;

    #endregion

    CardUseCnt cardCnt;
    public int AttackCnt { get { return cardCnt.attack; } set { cardCnt.attack = value; if (attackUseEvent != null) attackUseEvent.Invoke(); } }
    public int SkillCnt { get { return cardCnt.skill; } set { cardCnt.skill = value; if (skillUseEvent != null) skillUseEvent.Invoke(); } }
    public int PowerCnt { get { return cardCnt.power; } set { cardCnt.power = value; if (powerUseEvent != null) powerUseEvent.Invoke(); } }
    public int Money { get { return data.money; } set { data.money = value; GameManager.instance.allMoney = value;  } }

    void Awake()
    {
        isPlayer = true;

        battleStart += CardCountReset;
        battleStart += DataInit;
        SettingAnime();
        LoadInvenData();
        LoadPlayerData();

        artifactList.Add("BurningBlood");
        artifactList.Add("Meat");
        artifactList.Add("Marbles");
        //artifactList.Add("Mark_of_pain");
        artifactList.Add("Lantern");
        //artifactList.Add("Blood_vial");
        //artifactList.Add("Vajra");
        //artifactList.Add("OldCoin");
    }

    public void CardCountReset()
    {
        cardCnt.attack = 0;
        cardCnt.skill = 0;
        cardCnt.power = 0;
    }

    void CreateInvenList()
    {
        for (int i = 0; i < 5; i++)
        {
            inventoryList.Add("Strike");
        }
        for (int i = 0; i < 4; i++)
        {
            inventoryList.Add("Defend");
        }
        inventoryList.Add("Bash");
        //inventoryList.Add("Armaments");
       // inventoryList.Add("Inflame");
        //inventoryList.Add("Heavy_blade");
        //inventoryList.Add("Wild_strike");
        //inventoryList.Add("Flex");
        //inventoryList.Add("Body_slam");
    }

    void LoadPlayerData()
    {
        if (JsonManager.CheckJsonData(gameObject.name, gameObject.name))
        {
            data = JsonManager.LoadJsonData<CharacterData>(gameObject.name, gameObject.name);
        }
        else
        {
            IroncladSetting();
            JsonManager.SaveJsonData(data, gameObject.name, gameObject.name);
        }
    }
    void SettingAnime()
    {
        spain = GetComponent<SkeletonAnimation>();
        spain.loop = true;
        spain.AnimationName = "";
        spain.Awake();
        spain.Initialize(true);
        AnimeChangeStart("Idle", true);
    }

    void LoadInvenData()
    {
        if (JsonManager.CheckJsonData(gameObject.name, gameObject.name + "ItemList"))
        {
            inventoryList = JsonManager.LoadJsonData<List<string>>(gameObject.name, gameObject.name + "ItemList");
        }
        else
        {
            CreateInvenList();
            JsonManager.SaveJsonData(inventoryList, gameObject.name, gameObject.name + "ItemList");
        }
    }

    void IroncladSetting()
    {
        data.maxHP = 80;
        data.currentHP = data.maxHP;
        Money = 100;
    }

    public void BattleEnd()
    {
        if (battleEnded.GetInvocationList().Length <= 0)
        {
            return;
        }
        battleEnded.Invoke();
    }

    public void BattleStart()
    {
        if (battleStart == null)
        {
            return;
        }
        if (battleStart.GetInvocationList().Length <= 0)
        {
            return;
        }
        battleStart.Invoke();
    }

    public override void Hit(float damage)
    {
        base.Hit(damage);
        AnimeOneShotStart("Hit");
        StartCoroutine(HitPanelOn());
        if (data.currentHP<=0)
        {
            Dead();
        }
    }
    void Dead()
    {
        FindObjectOfType<EndingManager>().Die();
    }
    
    IEnumerator HitPanelOn()
    {
        hitPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitPanel.SetActive(false);
    }
}

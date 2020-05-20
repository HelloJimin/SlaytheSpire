using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;

public class Character : MonoBehaviour
{
    public CharacterData data;
    public bool isPlayer;
    public int vulnerable;
    public int weak;
    private int frail;

    public Dictionary<string , StatusUI> statusUIs = new Dictionary<string, StatusUI>();

    #region 프로퍼티

    public int Vulnerable
    {
        get
        {
            return vulnerable;
        }
        set
        {
            vulnerable = value;
            PropertySet("vulnerable", Resources.LoadAll<Sprite>("Sprite/powers")[109], vulnerable);
        }
    }

    public int Weak
    {
        get
        {
            return weak;
        }
        set
        {
            weak = value;
            PropertySet("weak", Resources.LoadAll<Sprite>("Sprite/powers")[147], weak);
        }
    }

    public int Frail
    {
        get
        {
            return frail;
        }
        set
        {
            frail = value;
            PropertySet("frail", Resources.LoadAll<Sprite>("Sprite/powers")[64] , frail);
        }
    }

    public int Power
    {
        get
        {
            return data.power;
        }
        set
        {
            data.power = value;
            PropertySet("power", Resources.LoadAll<Sprite>("Sprite/powers")[112] , data.power );
        }
    }

    public int Dexterity
    {
        get
        {
            return data.dexterity;
        }
        set
        {
            data.dexterity = value;
            PropertySet("dexterity", Resources.LoadAll<Sprite>("Sprite/powers")[18], data.dexterity);
        }
    }
    #endregion

    public delegate void MyTurnEnd();
    public event MyTurnEnd myTurnEnd;
    public delegate void MyTurnStart();
    public event MyTurnStart myTurnStart;

    public SkeletonAnimation anime;

    public Text HPText;

    public GameObject statusPanel;

    public virtual void Start()
    {
        HPText = transform.Find("Canvas/MyHP/HPText").GetComponent<Text>();
        anime = GetComponent<SkeletonAnimation>();
        statusPanel = transform.Find("Canvas/StatusPanel").gameObject;
        myTurnEnd += CoolDown;
        SettingHPUI();
    }

    public virtual void Hit(float damage)
    {
        damage = DefenseDamageCheck(damage);

        data.currentHP -= (int)damage;
        SettingHPUI();
        //ShieldBreak();
        UIManager.instance.CameraShake();
    }

    public int DefenseDamageCheck(float damage)
    {
        float Damage = damage;

        if (vulnerable > 0) Damage *= 1.5f;

        if (data.shield>0)
        {
            float temp = Damage;
            Damage -= data.shield;
            data.shield -= (int)temp;
            ShieldBreak();
            if (data.shield <= 0)
            {
                data.shield = 0;
                SettingShieldUI(false);
            }
            else
            {
                SettingShieldUI(true);
            }
        }

        if (Damage < 0) Damage = 0;

        return (int)Damage;
    }
    public void GetHeal(int heal)
    {
        if ((data.currentHP + heal) >= data.maxHP)
        {
            data.currentHP = data.maxHP;
        }
        else
        {
            data.currentHP += heal;
        }

        UIManager.instance.SettingUI();
    }

    public void GetShield(int sheild)
    {
        data.shield += sheild + data.dexterity;
        if (frail > 0) data.shield -= (int)(data.shield * 0.25f);

        SettingShieldUI(true);
    }

    public void ShieldBreak(int damage = 0)
    {
        if (data.shield > 0) return;

        SettingShieldUI(false);
    }

    void SettingShieldUI(bool isShield)
    {
        if (isShield)
        {
            transform.Find("Canvas").transform.Find("MyHP").GetComponent<Outline>().enabled = true;
            transform.Find("Canvas/MyHP/HPBar").GetComponent<Image>().color = Color.blue;
            transform.Find("Canvas/MyHP/SheildImage").gameObject.SetActive(true);
            transform.Find("Canvas/MyHP/SheildImage").GetComponentInChildren<Text>().text = data.shield.ToString();
        }
        else
        {
            transform.Find("Canvas/MyHP/SheildImage").gameObject.SetActive(false);
            transform.Find("Canvas/MyHP/HPBar").GetComponent<Image>().color = Color.red;
            transform.Find("Canvas").transform.Find("MyHP").GetComponent<Outline>().enabled = false;
        }
    }

    public void MyEndPhase()
    {
        myTurnEnd.Invoke();
    }

    public virtual void YourEndPhase()
    {
        data.shield = 0;
        ShieldBreak();
    }

    public void MyStartPhase()
    {
        myTurnStart.Invoke();
    }

    void CoolDown()
    {
        if (Frail > 0) Frail--;
        if (Vulnerable > 0) Vulnerable--;
        if (Weak > 0) Weak--;
    }



    public virtual void Animation(string aniName, bool isLeft)
    {
        float position;

        if (isLeft)
        {
            position = -0.5f;
        }
        else
        {
            position = 1;
        }

        if (aniName == "Atk")
        {
            transform.DOMove(new Vector3(transform.position.x + position, transform.position.y, transform.position.z), 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Flash);
        }
        else
        {

        }
    }

    public void SettingHPUI()
    {
        HPText.text = data.currentHP + "/" + data.maxHP;
        HPText.transform.parent.Find("HPBar").GetComponent<Image>().fillAmount = (float)data.currentHP / (float)data.maxHP;
    }

    void PropertySet(string Key , Sprite sprite, int value)
    {
        string key = Key;

        if (!statusUIs.ContainsKey(key))
        {
            StatusUI newStatusUI = ObjectPoolManager.instance.GetStatusUI(transform.Find("Canvas/StatusPanel").gameObject, sprite);
            statusUIs.Add(key, newStatusUI);
        }

        if (statusUIs.ContainsKey(key))
        {
            statusUIs[key].SettingUI(value);
        }

        if (value <= 0)
        {
            value = 0;
            if (statusUIs.ContainsKey(key))
            {
                ObjectPoolManager.instance.ReturnStatusUI(statusUIs[key]);
                statusUIs.Remove(key);
            }
        }
    }

    public void DataInit()
    {
        data.dexterity = 0;
        data.power = 0;
        Vulnerable = 0;
        Weak = 0;
        Frail = 0;
    }
}

[System.Serializable]
public struct CharacterData
{
    public string name;
    public int currentHP;
    public int maxHP;

    public int shield;
    public int power;
    public int dexterity;
    public int money;
}
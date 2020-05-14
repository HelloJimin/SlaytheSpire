using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ChoiceMode
{
    Grab,
    Upgrade,
    Choice
}


public class UIManager : MonoBehaviour
{
    #region 싱글톤
    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {
        get
        {
            if (m_instance == null) // 싱글톤 변수에 오브젝트가 할당되지 않았다면
            {
                // 씬에서 게임매니저 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance;
    #endregion

    public GameObject alphaImage;
    public GameObject usedCardAnime;
    public Text costText;
    public Text myDeckText;
    public Text myCemeteryText;
    public Text myHPText;
    public Text myMoneyText;

    public Image AtkEffect;
    public bool isChoiceMode;
    public GameObject choicePanel;
    public int choiceSize;

    public GameObject battleSystem;
    public GameObject battleUI;
    public GameObject mapScroll;
    public GameObject Neow;
    public GameObject reward;

    public ChoiceMode choice;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
    }

    public void SettingUI()
    {
        costText.text = GameManager.instance.currentCost + "/" + GameManager.instance.maxCost;
        myDeckText.text = GameManager.instance.myDeck.transform.childCount.ToString();
        myCemeteryText.text = GameManager.instance.myCemetary.transform.childCount.ToString();
        myHPText.text = GameManager.instance.player.data.currentHP + "/"+ GameManager.instance.player.data.maxHP;
        myMoneyText.text = GameManager.instance.player.data.money.ToString();
        GameManager.instance.player.SettingHPUI();
    }

    public IEnumerator Effect(string effName , Transform target)
    {
        AtkEffect.gameObject.SetActive(true);
        AtkEffect.sprite = Resources.Load<Sprite>("Sprite/Cards/atkEffects/" + effName) as Sprite;
        AtkEffect.transform.position = target.position+ new Vector3(0,1.5f,0);

        yield return new WaitForSeconds(0.1f);
        AtkEffect.gameObject.SetActive(false);
    }

    public void EffectStart(string effName, Transform target)
    {
        StartCoroutine(Effect(effName, target));
    }

    public void ChoiceMode(bool yesORno, int ChoiceSize = 0)
    {
        isChoiceMode = yesORno;
        alphaImage.SetActive(yesORno);
        alphaImage.transform.Find("OkButton").gameObject.SetActive(yesORno);
        alphaImage.transform.Find("ChoicePanel").gameObject.SetActive(yesORno);
        choiceSize = ChoiceSize;
    }

    public void GoToNeowRoom()
    {
        battleUI.SetActive(false);
    }
    public void GoToMap()
    {
        alphaImage.SetActive(true);
        mapScroll.SetActive(true);
        battleSystem.SetActive(false);
    }

    public void GoToMonsterRoom()
    {
        Neow.SetActive(false);
        mapScroll.SetActive(false);
        alphaImage.SetActive(false);

        battleSystem.SetActive(true);
        battleUI.SetActive(true);
        GameManager.instance.StartGame();
    }
}

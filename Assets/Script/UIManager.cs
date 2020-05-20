using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Map;
public enum ChoiceMode
{
    None,
    Grab,
    Upgrade,
    Choice,
    RestUpgrade
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
    public GameObject costUI;
    public GameObject proceedButton;
    public Image[] costUIs;

    public ChoiceMode choice;

    public Camera camera;

    public Text RewardbanerText;
    public GameObject scollMapUI;
    public GameObject restRoom;
    public GameObject restUpgradePanel;

    public GameObject turnEndButton;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        camera = FindObjectOfType<Camera>();
    }

    void Start()
    {
        costUIs = costUI.GetComponentsInChildren<Image>();
        StartCoroutine(Bingle());
        choice = global::ChoiceMode.Grab;

        //    reward.GetComponent<Button>().GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
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
        GameManager.instance.player.DataInit();
        choice = global::ChoiceMode.Grab;

        Neow.SetActive(false);
        alphaImage.SetActive(false);
        scollMapUI.gameObject.SetActive(true);

        //mapScroll.SetActive(true);
        battleSystem.SetActive(false);
        proceedButton.SetActive(false);
        restRoom.SetActive(false);

        Reward[] rewards = reward.transform.Find("Panel").GetComponentsInChildren<Reward>();
        for (int i = 0; i < rewards.Length; i++)
        {
            ObjectPoolManager.instance.ReturnRewardButton(rewards[i]);
        }
        reward.SetActive(false);
    }

    public void GoToRestRoom()
    {
        alphaImage.SetActive(true);
        battleSystem.SetActive(false);
        proceedButton.SetActive(false);
        restRoom.SetActive(true);
    }

    public void GoToMonsterRoom()
    {
        scollMapUI.SetActive(false);
        Neow.SetActive(false);
       // mapScroll.SetActive(false);
        alphaImage.SetActive(false);

        Debug.Log("몬스터룸으로 갑니다");
        battleSystem.SetActive(true);
        battleUI.SetActive(true);
        GameManager.instance.StartGame();
    }

    public void CameraShake()
    {
        Vector3 endPosition = new Vector3(camera.transform.position.x + 0.2f, camera.transform.position.y + 0.1f, camera.transform.position.z);
        camera.transform.DOMove(endPosition, 0.07f).SetLoops(2, LoopType.Yoyo);
    }

    IEnumerator Bingle()
    {
        costUIs[2].rectTransform.Rotate(Vector3.forward * Time.deltaTime * 150f);
        costUIs[3].rectTransform.Rotate(Vector3.forward * Time.deltaTime * -150f);
        costUIs[4].rectTransform.Rotate(Vector3.forward * Time.deltaTime * 130f);

        yield return new WaitForSeconds(0.05f);
        StartCoroutine(Bingle());
    }

    public void UsedCardAnimeStart(GameObject target)
    {
        usedCardAnime.gameObject.SetActive(true);

        Vector3 original = transform.position;
        Vector3 endPosition = target.transform.position;

        usedCardAnime.transform.DOMove(endPosition, 0.7f)
                .OnComplete(() =>
                {
                    usedCardAnime.transform.position = original;
                    usedCardAnime.gameObject.SetActive(false);
                });
    }

    public void OpenRewardPanel()
    {
        battleUI.SetActive(false);
        battleSystem.SetActive(false);
        proceedButton.SetActive(true);

        alphaImage.SetActive(true);
        reward.SetActive(true);
        reward.transform.Find("Panel").gameObject.SetActive(true);
        reward.transform.Find("ChoicePanel").gameObject.SetActive(false);
        RewardbanerText.text = "전리품!";
    }

    public void CloseRewardPanel()
    {
        GameManager.instance.DeckReset(reward.transform.Find("ChoicePanel").gameObject);

        proceedButton.SetActive(false);
        reward.SetActive(false);
        alphaImage.SetActive(false);
    }

    public void SucceesChoice()
    {
        choice = global::ChoiceMode.Grab;
        CloseRewardPanel();
        OpenRewardPanel();
    }

    public void RestCardGoToUpgradePanel(Card card)
    {
        restUpgradePanel.SetActive(true);
        choice = global::ChoiceMode.None;
        GameObject choicePanel = restUpgradePanel.transform.Find("ChoicePanel").gameObject;

        card.transform.SetParent(choicePanel.transform);
        Debug.Log(choicePanel+"여기실행");
        Card NewCard = ObjectPoolManager.instance.GetCard(card.GetType() + "+");
        NewCard.transform.SetParent(choicePanel.transform);
        NewCard.gameObject.SetActive(true);
    }

    public void RestCardGoToCardPool()
    {
        GameObject choicePanel = restUpgradePanel.transform.Find("ChoicePanel").gameObject;
        Card[] cards = choicePanel.GetComponentsInChildren<Card>();
        for (int i = 0; i < cards.Length; i++)
        {
            ObjectPoolManager.instance.ReturnCard(cards[i]);
        }
    }
}

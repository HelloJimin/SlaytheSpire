using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class ObjectPoolManager : MonoBehaviour
{
    #region 싱글톤
    // 싱글톤 접근용 프로퍼티
    public static ObjectPoolManager instance
    {
        get
        {
            if (m_instance == null) // 싱글톤 변수에 오브젝트가 할당되지 않았다면
            {
                // 씬에서 게임매니저 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<ObjectPoolManager>();
            }

            return m_instance;
        }
    }

    private static ObjectPoolManager m_instance;
    #endregion

    public GameObject cardPrefab;
    public GameObject monsterPrefab;
    public GameObject artifactPrefab;
    public GameObject rewardButtonPrefab;
    public GameObject statusUIPrefab;

    public List<string> cardList = new List<string>();
    public List<string> artifactList = new List<string>();

    List<Card> cardPool = new List<Card>();
    List<Character> monsterPool = new List<Character>();
    public List<Artifact> artifactPool = new List<Artifact>();
    Queue<Reward> rewardButtonPool = new Queue<Reward>();
    Queue<StatusUI> statusUIPool = new Queue<StatusUI>();

    void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        string[] monsterFiles = { "NobGoblin" , "Cultist" };

        for (int i = 0; i < monsterFiles.Length; i++)
        {
          //  monsterPool.Add(CreateNewMonster(monsterFiles[i]));
        }
        artifactList.Add("BurningBlood");
        artifactList.Add("Blood_vial");
        artifactList.Add("OldCoin");
        artifactList.Add("Vajra");
        artifactList.Add("Orichalcum");
        artifactList.Add("Meat");
        artifactList.Add("Marbles");
        artifactList.Add("Strawberry");
        artifactList.Add("Mango");
        artifactList.Add("Mark_of_pain");
        artifactList.Add("Lantern");
        for (int i = 0; i < artifactList.Count; i++)
        {
            artifactPool.Add(CreateArtifact(artifactList[i]));
        }

        for (int i = 0; i < 10; i++)
        {
            statusUIPool.Enqueue(CreateStatusUI());
        }

        for (int i = 0; i < 5; i++)
        {
            rewardButtonPool.Enqueue(CreateButtons());
        }

        cardList.Add("Bash");
        cardList.Add("Armaments");
        cardList.Add("Inflame");
        cardList.Add("Heavy_blade");
        cardList.Add("Wild_strike");
        cardList.Add("Flex");
        cardList.Add("Body_slam");
        cardList.Add("Strike");
        cardList.Add("Defend");
        cardList.Add("Wound");


        for (int i = 0; i < cardList.Count; i++)
        {
            for (int k = 0; k < 3; k++)
            {
               cardPool.Add(CreateCard(cardList[i]));
            }
        }
    }

    #region CREATE

    StatusUI CreateStatusUI()
    {
        StatusUI newUI = Instantiate(statusUIPrefab, transform.Find("StatusUIs")).GetComponent<StatusUI>();

        newUI.gameObject.SetActive(false);
        return newUI;
    }

    Artifact CreateArtifact(string name)
    {
        GameObject test = Instantiate(artifactPrefab, transform.Find("Artifacts"));
        test.AddComponent(System.Type.GetType(name));

        Artifact newArtifact = test.GetComponent<Artifact>();
       // newArtifact.transform.localPosition = new Vector3(2, 2, 2);
        newArtifact.Init();
        newArtifact.gameObject.SetActive(false);
        return newArtifact;
    }

    Card CreateCard(string name)
    {
        GameObject test = Instantiate(cardPrefab, transform.Find("Cards"));
        test.AddComponent(System.Type.GetType(name));

        Card newCard = test.GetComponent<Card>();
       // newCard.transform.localScale = new Vector3(2, 2, 2);
        newCard.cardInit();
        newCard.gameObject.SetActive(false);

        return newCard;
    }

    Reward CreateButtons()
    {
        Reward newButton = Instantiate(rewardButtonPrefab, transform.Find("Buttons")).GetComponent<Reward>();

        newButton.gameObject.SetActive(false);
        return newButton;
    }

    Character CreateNewMonster(string monsterFileName)
    {
        Debug.Log(monsterFileName+ " 생성 완료");
        Character newMon = Instantiate(monsterPrefab, transform.Find("Monsters")).AddComponent(System.Type.GetType(monsterFileName)).GetComponent<Character>();
        newMon.gameObject.SetActive(false);
        newMon.name = monsterFileName;
        return newMon;
    }

    #endregion

    #region GET
    public Artifact GetArtifact(string artifactName)
    {
        Artifact artifact;
        if (artifactPool.Count > 0)
        {
            for (int i = 0; i < artifactPool.Count; i++)
            {
                if (artifactPool[i].GetType().Name == artifactName)
                {
                    artifact = artifactPool[i];
                    artifact.gameObject.SetActive(false);
                    artifactPool.Remove(artifact);

                    return artifact;
                }
            }
            artifact = CreateArtifact(artifactName);
            return artifact;
        }
        artifact = CreateArtifact(artifactName);
        return artifact;
    }

    public Artifact GetArtifact(bool isRandom)
    {
        Artifact artifact;
        if (artifactPool.Count > 0)
        {
            artifact = artifactPool[ Random.Range(0,artifactPool.Count)];
            artifact.gameObject.SetActive(false);
            artifactPool.Remove(artifact);
            return artifact;
        }
        else
        {
            return default;
        }
    }

    public Card GetCard(string cardName)
    {
        Card card;
        if (cardPool.Count > 0)
        {
            for (int i = 0; i < cardPool.Count; i++)
            {
                if (cardPool[i].GetType().Name == cardName)
                {
                    card = cardPool[i];
                    card.gameObject.SetActive(false);
                    cardPool.Remove(card);
                    return card;
                }
            }

            if (cardName.Contains("+"))
            {
                card = CreateCard(cardName.Replace("+",""));
                card.CardUpgrade();
                return card;
            }
            else
            {
                card = CreateCard(cardName);
                return card;
            }
        }

        if (cardName.Contains("+"))
        {
            card = CreateCard(cardName.Replace("+", ""));
            card.CardUpgrade();
            return card;
        }
        else
        {
            card = CreateCard(cardName);
            return card;
        }
    }

    public Character GetMonster(string monsterName)
    {
        Character mon;

        if (monsterPool.Count > 0)
        {
            for (int i = 0; i < monsterPool.Count; i++)
            {
                if (monsterPool[i].name == monsterName)
                {
                    Debug.Log("몬스터풀 가져오기 완료");
                    mon = monsterPool[i];
                    mon.gameObject.SetActive(true);
                    monsterPool.Remove(monsterPool[i]);
                    return mon;
                }
            }
            Debug.Log("몬스터 풀에 몬스터가 부족해서 생성합니다");
            mon = CreateNewMonster(monsterName);
            return mon;
        }
        Debug.Log("몬스터풀이 아예 비어있어서 생성합니다");
        mon = CreateNewMonster(monsterName);
        return mon;
    }

    public void GetRewardMoneyButton(string type)
    {
        Reward button = rewardButtonPool.Dequeue();
        button.transform.SetParent(UIManager.instance.reward.transform.Find("Panel").transform);
        button.gameObject.SetActive(true);
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        button.transform.localScale = new Vector3(1, 1, 1);
        button.init(type);
    }
    public StatusUI GetStatusUI(GameObject statusPanel, Sprite sprite )
    {
        StatusUI statusUI = statusUIPool.Dequeue();
        statusUI.transform.position = new Vector3(0, 0, -1);
        statusUI.transform.SetParent(statusPanel.transform);
        statusUI.gameObject.SetActive(true);
        statusUI.Init(sprite);
        return statusUI;
    }
    #endregion

    #region RETURN

    public void ReturnArtifact(Artifact artifact)
    {
        artifactPool.Add(artifact);
        artifact.gameObject.SetActive(false);
        artifact.transform.SetParent(transform.Find("Artifacts"));
    }

    public void ReturnMonster(Character monster)
    {
        monster.gameObject.SetActive(false);
        monster.transform.SetParent(transform.Find("Monsters"));
        monsterPool.Add(monster);
    }

    public void ReturnCard(Card card)
    {
        cardPool.Add(card);
        // card.cardInit();
        card.gameObject.SetActive(false);
        card.transform.SetParent(transform.Find("Cards"));
    }

    public void ReturnRewardButton(Reward button)
    {
        rewardButtonPool.Enqueue(button);
        button.gameObject.SetActive(false);
        button.transform.SetParent(transform.Find("Buttons"));
    }
    public void ReturnStatusUI(StatusUI ui)
    {
        statusUIPool.Enqueue(ui);
        ui.gameObject.SetActive(false);
        ui.transform.SetParent(transform.Find("StatusUIs"));
    }
    #endregion

}

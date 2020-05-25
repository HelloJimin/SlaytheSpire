using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Heart : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public Button button;
    private void OnEnable()
    {
        textMeshPro.text = "<shake>깊이 고동치는 공포가 방 전체에 걸쳐 느껴집니다.\n이것이 첨탑의 심장일까요?\n당신은 대검을 준비합니다...";
        button.GetComponentInChildren<Text>().text = "공격";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Attack);
    }

    public void Attack()
    {
        textMeshPro.text = "<shake>심장이 꿈틀대며 피를 흘립니다...\n하지만 심장은 아직도 뛰고 있습니다.\n혼신의 공격이었는데도 부족했던 걸까요?\n심장은 더욱 더 크게 맥박치고\n당신의 의식은 점점 흐릿해집니다...";
        //샤샤샥
        for (int i = 0; i < 3; i++)
        {
            AtkSound(i * 0.2f);
        }
        button.GetComponentInChildren<Text>().text = "수면";
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Sleep);
    }

    public void Sleep()
    {
        EndingManager endingManager = FindObjectOfType<EndingManager>();
        endingManager.endingUI.SetActive(true);
        endingManager.Win();

        gameObject.SetActive(false);
    }

    IEnumerator AtkSound(float time)
    {
        yield return new WaitForSeconds(time);
        SoundManager.instance.PlaySound("FastAtk1");
    }
}

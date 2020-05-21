using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestRoom : MonoBehaviour
{
    public Image image;
    public Sprite sprite1;
    public Sprite sprite2;
    public string spriteName;
    public bool isClick;

    private void OnEnable()
    {
        isClick = false;
        image = GetComponent<Image>();
        image.sprite = sprite1;
    }
    public void OnClick()
    {
        if (isClick)
        {
            return;
        }

        isClick = true;
        image.sprite = sprite2;
        UIManager.instance.proceedButton.SetActive(true);
        ObjectPoolManager.instance.GetRewardMoneyButton("artifact");
        UIManager.instance.alphaImage.SetActive(true);
        UIManager.instance.reward.SetActive(true);
    }
}

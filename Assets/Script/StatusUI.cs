using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public Image image;
    public Text text;
    public Character character;
    public void Init(Sprite sprite)
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        image.sprite = sprite;
    }

    public void SettingUI(int value)
    {
        text.text = value.ToString();
    }
}

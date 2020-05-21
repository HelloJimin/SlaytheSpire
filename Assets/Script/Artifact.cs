using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Artifact : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler
{
    public Sprite sprite;
    public Player player;
    public ArtifactData data;

    private MouseDescription mouseDescription;
    public int mPosY;
    public virtual void Init()
    {
        LoadData();

        player = FindObjectOfType<Player>();
        sprite = Resources.Load<Sprite>("Sprite/Artifact/" + GetType().Name) as Sprite;
        GetComponent<Image>().sprite = sprite;
        gameObject.name = data.name;
    }

    public void LoadData()
    {
        if (JsonManager.CheckJsonData(gameObject.name, gameObject.name))
        {
            data = JsonManager.LoadJsonData<ArtifactData>("Artifact", gameObject.name);
        }
        else
        {
            SettingArtifactData();
            JsonManager.SaveJsonData(data, "Artifact", gameObject.name);
        }
    }

    public virtual void SettingArtifactData()
    {
        data.value = 0;
        data.name = "";
        data.description = "";
        data.grade = ArtifactGrade.None;
    }

    public virtual void ActiveEffect()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mouseDescription == null)
        {
            mouseDescription = UIManager.instance.mouseDescription;
        }
        mouseDescription.mousePosY = mPosY;
        mouseDescription.LookDiscription(true,data.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (mouseDescription == null)
        {
            mouseDescription = UIManager.instance.mouseDescription;
        }
        mouseDescription.LookDiscription(false, data.description);
    }
}

public enum ArtifactGrade
{
    None,
    start,
    Nomal,
    Elite,
    Shop,
    Boss
}

[System.Serializable]
public struct ArtifactData
{
    public int value;
    public string name;
    public string description;
    public ArtifactGrade grade;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artifact : MonoBehaviour
{
    public Sprite sprite;
    public Player player;
    public ArtifactData data;

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
    }

    public virtual void ArtifactEffect()
    {

    }
}

[System.Serializable]
public struct ArtifactData
{
    public int value;
    public string name;
    public string description;
}
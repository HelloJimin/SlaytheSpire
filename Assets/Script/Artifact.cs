using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    public Sprite sprite;
    public Player player;

    void Start()
    {
    }

    public virtual void Init()
    {
        player = GameManager.instance.player;

    }
}
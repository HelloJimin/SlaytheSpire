﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LobbyManager : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }
    public void GoToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}

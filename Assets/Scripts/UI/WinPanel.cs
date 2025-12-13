using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinPanel : Panel
{
    [SerializeField] private Button returnButton;
    [SerializeField] private Button nextLevelButton;

    public void Awake()
    {
        nextLevelButton.onClick.AddListener(OnClickNextLevel);
        returnButton.onClick.AddListener(OnClickReturn);
    }

    public void OnClickNextLevel()
    {
        GameManager.instance.SetState(GameManager.GameState.IN_GAME);
        GameManager.instance.LoadNextLevel();
    }

    public void OnClickReturn()
    {
        GameManager.instance.SetState(GameManager.GameState.LEVEL);
    }
}

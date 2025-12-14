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
        AudioManager.instance.PlaySfx("buttonClick", 0.3f);

        if (GameManager.instance.HaveNextLevel())
        {
            GameManager.instance.SetState(GameManager.GameState.IN_GAME);
            GameManager.instance.LoadNextLevel();
        }
    }

    public void OnClickReturn()
    {
        AudioManager.instance.PlaySfx("buttonClick", 0.3f);
        GameManager.instance.SetState(GameManager.GameState.LEVEL);
    }
}

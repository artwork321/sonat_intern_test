using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LosePanel : Panel
{
    [SerializeField] private Button returnButton;

    public void Awake()
    {
        returnButton.onClick.AddListener(OnClickReturn);
    }

    public void OnClickReturn()
    {
        GameManager.instance.SetState(GameManager.GameState.LEVEL);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPanel : Panel
{
    [SerializeField] private GameObject levelGrid;
    [SerializeField] private GameObject levelButtonPrefab;

    [SerializeField] private Button returnButton;

    public void Awake()
    {
        GameSettings[] levels = Resources.LoadAll<GameSettings>("Levels");

        for (int i = 0; i < levels.Length; i++)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab, transform.position, transform.rotation, levelGrid.transform);
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
            int levelIndex = i + 1;
            levelButton.GetComponent<Button>().onClick.AddListener(() => OnClickLevel(levelIndex));
        }

        returnButton.onClick.AddListener(OnClickReturn);

    }

    public void OnClickLevel(int level)
    {
        GameManager.instance.SetState(GameManager.GameState.IN_GAME);
        GameManager.instance.LoadLevel(level);
    }

    public void OnClickReturn()
    {
        GameManager.instance.SetState(GameManager.GameState.MENU);
    }
}

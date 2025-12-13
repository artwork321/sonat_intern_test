using UnityEngine;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour
{
    List<Panel> panels;

    [SerializeField] private Panel MenuPanel;
    [SerializeField] private Panel LevelPanel;
    [SerializeField] private Panel GamePanel;
    [SerializeField] private Panel WinPanel;
    [SerializeField] private Panel LosePanel;

    public void Awake()
    {
        panels = new List<Panel>();

        panels.Add(MenuPanel);
        panels.Add(LevelPanel);
        panels.Add(GamePanel);
        panels.Add(WinPanel);
        panels.Add(LosePanel);

        foreach (Panel panel in panels)
        {
            panel.Hide();
        }

        // Start the game by showing the menu
        ShowPanel(MenuPanel);
    }

    public void SwitchGameState(GameManager.GameState m_state)
    {
        switch (m_state)
        {
            case GameManager.GameState.MENU:
                ShowPanel(MenuPanel);
                break;

            case GameManager.GameState.LEVEL:
                ShowPanel(LevelPanel);
                break;

            case GameManager.GameState.IN_GAME:
                ShowPanel(GamePanel);
                break;

            case GameManager.GameState.WIN:
                ShowPanel(WinPanel);
                break;

            case GameManager.GameState.LOSE:
                ShowPanel(LosePanel);
                break;
        }
    }

    private void ShowPanel(Panel targetPanel)
    {
        foreach (Panel panel in panels)
        {
            if (targetPanel == panel) targetPanel.Show();
            else panel.Hide();
        }

    }
}

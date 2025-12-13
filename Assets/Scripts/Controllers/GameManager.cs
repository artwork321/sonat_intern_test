using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameSettings settings;

    private int currentLevel;

    private PanelManager panelManager;

    public enum GameState
    {
        MENU,
        LEVEL,
        IN_GAME,
        LOSE,
        WIN
    }

    private GameState gameState;

    public GameState State
    {
        get { return gameState; }
        private set
        {
            gameState = value;
            panelManager.SwitchGameState(gameState);
        }
    }


    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There are two BottleControllers!");
        }
        instance = this;

        panelManager = GameObject.FindFirstObjectByType<PanelManager>();
    }

    public void LoadLevel(int i)
    {
        // Load the right game configuration
        string levelString = "Levels/Level_" + i;
        settings = Resources.Load<GameSettings>(levelString);
        currentLevel = i;

        // Set up bottles
        BottleController.instance.SetupBottles();
    }

    public void LoadNextLevel()
    {
        // Clear old instances
        BottleController.instance.DestroyBottles();

        // Load new level
        LoadLevel(currentLevel + 1);
    }

    public void SetState(GameState state)
    {
        State = state;
    }

    public void IsWin()
    {
        List<Bottle> allBottles = BottleController.instance.bottles;

        foreach (Bottle bottle in allBottles)
        {
            if (!bottle.IsComplete() && !bottle.IsEmpty()) return;
        }

        SetState(GameState.WIN);
    }

    public void IsLose()
    {
        // Check if all top colors are different
        List<Bottle> allBottles = BottleController.instance.bottles;

        List<Color> allTopColors = allBottles.Select(bottle => bottle.GetTopWaterColor()).ToList();
        int uniqueColorCount = allTopColors.Distinct().Count();
        bool isThereEmptyBottle = allTopColors.Any(color => color == Color.clear);

        if (!isThereEmptyBottle && (allTopColors.Count == uniqueColorCount)) SetState(GameState.LOSE);
    }

    public void CheckEndGameCondition()
    {
        IsWin();
        IsLose();
    }

}

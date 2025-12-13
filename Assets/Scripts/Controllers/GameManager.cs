using UnityEngine;

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

        // Load new level
        LoadLevel(currentLevel + 1);
    }

    public void SetState(GameState state)
    {
        Debug.Log("Hello World");
        State = state;
    }


}

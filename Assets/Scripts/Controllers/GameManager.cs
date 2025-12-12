using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameSettings settings;

    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There are two BottleControllers!");
        }
        instance = this;

        settings = Resources.Load<GameSettings>("GameSettings");
    }

    public void LoadLevel()
    {
        // Set up bottles
        BottleController.instance.SetupBottles();
    }

}

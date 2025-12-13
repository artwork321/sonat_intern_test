using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : Panel
{
    [SerializeField] private Button startButton;

    public void Awake()
    {
        startButton.onClick.AddListener(OnCLickStart);
    }

    public void OnCLickStart()
    {
        GameManager.instance.SetState(GameManager.GameState.LEVEL);
    }
}

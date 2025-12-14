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
        AudioManager.instance.PlaySfx("buttonClick", 0.3f);
        GameManager.instance.SetState(GameManager.GameState.LEVEL);
    }
}

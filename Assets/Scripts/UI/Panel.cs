using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}

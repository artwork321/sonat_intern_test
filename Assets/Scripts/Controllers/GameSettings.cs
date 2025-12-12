using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Custom/Settings")]
public class GameSettings : ScriptableObject
{
    public float width = 5;

    public float height = 3;

    public int numberOfBottles = 8;
}

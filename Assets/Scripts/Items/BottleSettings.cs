using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class BottleSettings
{
    // These fields will now appear directly in the GameSettings Inspector
    public List<float> eachWaterAmount;
    public List<Color> eachColor;
}
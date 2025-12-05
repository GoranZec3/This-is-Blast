using UnityEngine;

public class Colorize : MonoBehaviour
{
    public BeColor color;

    //shooter uses
    public Color GetColorFromEnum(BeColor beColor)
    {
        switch (beColor)
        {
            case BeColor.Red: return Color.red;
            case BeColor.Green: return Color.green;
            case BeColor.Blue: return Color.blue;
            case BeColor.Yellow: return Color.yellow;
            case BeColor.Purple: return new Color(0.5f, 0f, 0.5f); // purple
            default: return Color.white;
        }
    }
}

public enum BeColor
{
    Red,
    Green,
    Blue,
    Yellow,
    Purple
}

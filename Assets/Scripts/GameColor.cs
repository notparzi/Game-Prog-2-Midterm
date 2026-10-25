using UnityEngine;

public enum GameColor { Red, Green, Blue }

public static class GameColorUtil
{
    public static Color ToColor(GameColor c)
    {
        if (c == GameColor.Red)
        {
            return Color.red;
        }
        else if (c == GameColor.Green)
        {
            return Color.green;
        }
        else
        {
            return Color.blue;
        }
    }
}
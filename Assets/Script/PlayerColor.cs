using System.Collections.Generic;
using UnityEngine;

public enum PlayerColorType
{
    Red, Blue, Green,
    Pink, Orange, Yellow,
    Black, White, Purple,
    Browm, Cyan, Lime
}

public class PlayerColor : MonoBehaviour
{
    private static List<Color> _playerColorList = new List<Color>()
    {
        new Color(1f, 0f, 0f),
        new Color(0.1f, 0.1f, 1f),
        new Color(0f, 0.6f, 0f),
        new Color(1f, 0.3f, 0.9f),
        new Color(1f, 0.4f, 0f),
        new Color(1f, 0.9f, 0.1f),
        new Color(0.2f, 0.2f, 0.2f),
        new Color(0.9f, 1f, 1f),
        new Color(0.6f, 0f, 0.6f),
        new Color(0.7f, 0.2f, 0f),
        new Color(0f, 1f, 1f),
        new Color(0.1f, 1f, 0.1f)
    };

    public static Color GetPlayerColor(PlayerColorType type)
    {
        return _playerColorList[(int)type];
    }
}

using UnityEngine;

public enum ControlType
{
    Mouse,
    KeyboardMouse
}

public class PlayerSettings
{
    public static ControlType _controlType;
    public static string _nickName;
}

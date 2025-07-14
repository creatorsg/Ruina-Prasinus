using UnityEngine;

public static class InputManager
{
    public static bool GetKey(string action)
        => Input.GetKey(KeybindManager.GetKeyCode(action));

    public static bool GetKeyDown(string action)
        => Input.GetKeyDown(KeybindManager.GetKeyCode(action));

    public static float GetAxisRaw(string axisName)
        => Input.GetAxisRaw(axisName);
}

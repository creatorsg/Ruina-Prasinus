using UnityEngine;
using System.Collections.Generic;

public static class KeybindManager
{
    private const string Prefix = "Keybind_";
    private static Dictionary<string, KeyCode> bindings;

    static KeybindManager()
    {
        LoadBindings();
    }

    private static void LoadBindings()
    {
        bindings = new Dictionary<string, KeyCode>();

        AddOrLoad("MoveLeft", KeyCode.A);
        AddOrLoad("MoveRight", KeyCode.D);
        AddOrLoad("LookUP", KeyCode.W);
        AddOrLoad("LieDown", KeyCode.S);
        AddOrLoad("Jump", KeyCode.Space);
        AddOrLoad("Dash", KeyCode.LeftShift);
        AddOrLoad("Attack", KeyCode.Mouse0);
        AddOrLoad("Pause", KeyCode.Escape);
    }

    private static void AddOrLoad(string action, KeyCode def)
    {
        string key = Prefix + action;
        if (PlayerPrefs.HasKey(key))
            bindings[action] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(key));
        else
        {
            bindings[action] = def;
            PlayerPrefs.SetString(key, def.ToString());
        }
    }

    public static KeyCode GetKeyCode(string action)
    {
        if (bindings.TryGetValue(action, out var kc))
            return kc;
        Debug.LogWarning($"[{action}]에 바인딩된 키가 없습니다.");
        return KeyCode.None;
    }

    public static void SetKey(string action, KeyCode key)
    {
        bindings[action] = key;
        PlayerPrefs.SetString(Prefix + action, key.ToString());
        PlayerPrefs.Save();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    // �⺻ ���ε�
    private static readonly Dictionary<string, KeyCode> defaultBindings = new Dictionary<string, KeyCode>
    {
        { "Jump", KeyCode.Space },
        { "Dash", KeyCode.LeftShift },
        { "Pause", KeyCode.Escape },
        // �ʿ��ϸ� �ٸ� �׼� �߰�
    };

    // ���� ��� ���� ���ε�
    private static Dictionary<string, KeyCode> bindings;

    static InputManager()
    {
        LoadBindings();
    }

    // PlayerPrefs���� �ҷ�����
    public static void LoadBindings()
    {
        bindings = new Dictionary<string, KeyCode>();
        foreach (var kv in defaultBindings)
        {
            string action = kv.Key;
            KeyCode def = kv.Value;
            string saved = PlayerPrefs.GetString("KeyBinding_" + action, def.ToString());
            if (Enum.TryParse(saved, out KeyCode kc))
                bindings[action] = kc;
            else
                bindings[action] = def;
        }
    }

    // �׼ǿ� Ű ���� ����
    public static void SaveBinding(string action, KeyCode key)
    {
        bindings[action] = key;
        PlayerPrefs.SetString("KeyBinding_" + action, key.ToString());
        PlayerPrefs.Save();
    }

    // �׼Ǻ� �Է� üũ
    public static bool GetKey(string action) => Input.GetKey(bindings[action]);
    public static bool GetKeyDown(string action) => Input.GetKeyDown(bindings[action]);

    // ���� ���ε��� Ű Ȯ��
    public static KeyCode GetBinding(string action) => bindings[action];
}

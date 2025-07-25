using System;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    // 기본 바인딩
    private static readonly Dictionary<string, KeyCode> defaultBindings = new Dictionary<string, KeyCode>
    {
        { "Jump", KeyCode.Space },
        { "Dash", KeyCode.LeftShift },
        { "Pause", KeyCode.Escape },
        // 필요하면 다른 액션 추가
    };

    // 실제 사용 중인 바인딩
    private static Dictionary<string, KeyCode> bindings;

    static InputManager()
    {
        LoadBindings();
    }

    // PlayerPrefs에서 불러오기
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

    // 액션에 키 새로 저장
    public static void SaveBinding(string action, KeyCode key)
    {
        bindings[action] = key;
        PlayerPrefs.SetString("KeyBinding_" + action, key.ToString());
        PlayerPrefs.Save();
    }

    // 액션별 입력 체크
    public static bool GetKey(string action) => Input.GetKey(bindings[action]);
    public static bool GetKeyDown(string action) => Input.GetKeyDown(bindings[action]);

    // 현재 바인딩된 키 확인
    public static KeyCode GetBinding(string action) => bindings[action];
}

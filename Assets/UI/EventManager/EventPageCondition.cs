using System.Collections.Generic;
using UnityEngine;

public class EventPageCondition
{
    private readonly Dictionary<string, bool> _switchOperation = new Dictionary<string, bool>();
    private SwitchManager _switchManager = SwitchManager.Instance;

    public EventPageCondition(Dictionary<string, bool> switchOperation) =>
        _switchOperation = switchOperation;

    public bool IsAllValid()
    {
        foreach (var kvp in _switchOperation)
            if (!IsAnyValid(kvp.Key, kvp.Value))
                return false;
        return true;
    }

    private bool IsAnyValid(string key, bool value)
    {
        if (_switchManager.SwitchTable.ContainsKey(key))
            return _switchManager.SwitchTable[key] == value;
        Debug.LogError($"EventPageCondition: \"{key}\" does not exist in \"SwitchTable\"!");
        return false;
    }
}
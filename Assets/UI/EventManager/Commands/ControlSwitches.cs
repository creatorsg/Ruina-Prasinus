using System.Collections.Generic;

public class ControlSwitchesCommand : EventCommandBase
{
    private SwitchManager _switchManager = SwitchManager.Instance;

    private static ControlSwitchesCommand _instance;
    public static ControlSwitchesCommand Instance => _instance;

    public void Execute(Dictionary<string, bool> switchOperation)
    {
        foreach (var (key, value) in switchOperation)
        {
            if (!_switchManager.SwitchTable.ContainsKey(key))
                continue;

            _switchManager.SetSwitchTable(key, value);
        }
    }
}
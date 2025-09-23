using System.Collections.Generic;
using UnityEngine;

public sealed class SwitchManager : MonoBehaviour
{
    private Dictionary<string, bool> _switchTable { get; set; } = new Dictionary<string, bool>();

    private SwitchCsvReader _csvReader { get; } = new SwitchCsvReader();
    private SwitchCsvOverwriter _csvOverwriter { get; } = new SwitchCsvOverwriter();

    public Dictionary<string, bool> SwitchTable => _switchTable;

    private static SwitchManager _instance;
    public static SwitchManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        ReadTable();
    }

    private void ReadTable()
    {
        _switchTable = _csvReader.ReadCsv();
    }

    public void OverwriteTable()
    {
        _csvOverwriter.OverWriteCsv(_switchTable);
    }

    public bool SetSwitchTable(string key, bool value)
    {
        if (!_switchTable.ContainsKey(key))
            return false;

        _switchTable[key] = value;
        return true;
    }
}
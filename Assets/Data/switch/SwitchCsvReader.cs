using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class SwitchCsvReader
{
    private string _filePath;

    public string FilePath => _filePath;

    public SwitchCsvReader(string path = "Data/switch/SwitchData.csv")
    {
        _filePath = path;
    }

    public Dictionary<string, bool> ReadCsv()
    {
        Dictionary<string, bool> switchTable = new Dictionary<string, bool>();

        using (StreamReader reader = new StreamReader(Application.dataPath + "/" + _filePath, true))
        {
            string data;
            while ((data = reader.ReadLine()) != null)
            {
                string[] splitData = data.Split(',');
                switchTable.Add(splitData[0], bool.Parse(splitData[1]));
            }
        }

        return switchTable;
    }

    private bool isFinish(string data)
    {
        if (data == null)
            return true;
        return false;
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class SwitchCsvOverwriter
{
    private string _filePath { get; set; }

    public string FilePath => _filePath;

    public SwitchCsvOverwriter(string path = "Data/switch/SwitchData.csv")
    {
        _filePath = path;
    }

    public void OverWriteCsv(Dictionary<string, bool> switchTable)
    {
        string fullPath = getFullPath(_filePath);

        if (!isExistDirectory(fullPath))
            throw new Exception("there's no switchCsv in the directory!");

        using (StreamWriter writer = new StreamWriter(fullPath, false))
            foreach (var content in switchTable)
                writer.WriteLine($"{content.Key},{content.Value}");

        return;
    }

    private string getFullPath(string path)
    {
        return Path.Combine(Application.dataPath, path);
    }

    private bool isExistDirectory(string path)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
            return false;
        return true;
    }
}
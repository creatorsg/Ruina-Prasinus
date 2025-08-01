using UnityEngine;

public static class VolumeSave
{
    private const string Prefix = "Audio_"; 

    public static float Load(string parameterName, float defaultDb)
    {
        string key = GetKey(parameterName);
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetFloat(key);
        return defaultDb;
    }

    public static void Save(string parameterName, float dB)
    {
        string key = GetKey(parameterName);
        PlayerPrefs.SetFloat(key, dB);
        PlayerPrefs.Save();
    }

    private static string GetKey(string parameterName) => $"{Prefix}{parameterName}_dB";
}

using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public static class MusicStorage
{
    private static Dictionary<string, string> _Audios = new Dictionary<string, string>()
    {
        {"Master", "bus:/Master" },
        {"BGM", "bus:/Master/BGM"},
        {"SE", "bus:/Master/SE"},
        {"ME", "bus:/Master/ME"},
        {"BGS", "bus:/Master/BGS"}
    };

    private static Dictionary<string, string> _BGMs = new Dictionary<string, string>()
    {
        {"BGM1", "event:/BGM/BGM" },

    };

    private static Dictionary<string, string> _SEs = new Dictionary<string, string>()
    {
        {"WalkSound", "event:/SE/Walk" },
        {"DashSound","event:/SE/Dash" },
        {"JumpSound","event:/SE/Jump" },
        {"AttackSound","event:/SE/Attack" }
    };

    private static Dictionary<string,string> _BGSs = new Dictionary<string, string>()
    {
        
    };

    private static Dictionary<string, string> _MEs = new Dictionary<string, string>()
    {
        
    };

    public static string SelectBUS(string bus) => _Audios[bus];
    public static string GetBGM(string music) => _BGMs[music];
    public static string GetSE(string sound) => _SEs[sound];
    public static string GetBGS(string sound) => _BGSs[sound];
    public static string GetME(string sound) => _MEs[sound];
}

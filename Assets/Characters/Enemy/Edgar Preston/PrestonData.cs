using UnityEngine;

[CreateAssetMenu(fileName = "PrestonData", menuName = "Scriptable Objects/PrestonData")]
public class PrestonData : ScriptableObject
{
    [Header("- º¸½º ½ºÅÈ -")]
    private float _bossHp = 100f;
    

    public float BossHp => _bossHp;
}

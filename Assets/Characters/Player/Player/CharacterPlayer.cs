using UnityEngine;

[CreateAssetMenu(fileName = "CharacterPlayer", menuName = "Scriptable Objects/CharacterPlayer")]
public class CharacterPlayer : ScriptableObject
{
    [Header("walk")]
    private float _walkAccelTime = 0.5f;
    private float _maxWalkSpeed = 5f;

    public float walkAccelTime => _walkAccelTime;
    public float maxWalkSpeed => _maxWalkSpeed;
}

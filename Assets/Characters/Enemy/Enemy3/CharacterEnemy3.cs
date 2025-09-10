using UnityEngine;

[CreateAssetMenu(fileName = "CharacterEnemy3", menuName = "Scriptable Objects/CharacterEnemy3")]
public class CharacterEnemy3 : ScriptableObject
{
    [Header("-- Detect Area --")]
    private float _detectDistance = 15f;

    [Header("-- Walk Area --")]
    private float _walkSpeed = 5f;

    [Header("-- Rush Area --")]
    private float _rushSpeed = 10f;


    public float DetectDistance => _detectDistance;
    public float WalkSpeed => _walkSpeed;
    public float RushSpeed => _rushSpeed;
}

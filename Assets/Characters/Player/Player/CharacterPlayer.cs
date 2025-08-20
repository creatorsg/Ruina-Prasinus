using UnityEngine;

[CreateAssetMenu(fileName = "CharacterPlayer", menuName = "Scriptable Objects/CharacterPlayer")]
public class CharacterPlayer : ScriptableObject
{
    private float _playerHp = 50f;

    [Header("walk")]
    private float _walkAccelTime = 0.1f;
    private float _maxWalkSpeed = 6.2f;


    [Header("dash")]
    private float _dashAccelTime = 0.1f;
    private float _maxDashSpeed = 10f;
    private float _remainDashTime = 0.5f;
    private float _dashCooltime = 3f;

    [Header("jump")]
    private float _jumpPower = 2f;
    private float _jumpAccelPower = 3.4f;
    private float _jumpRemainTime = 0.25f;

    private float _maxFallingSpeed = 3f;
    private float _fallingAccelTime = 0.25f;

    [Header("Attack")]
    private float _basicAttackPower = 2f;

    public float PlayerHp => _playerHp;

    public float WalkAccelTime => _walkAccelTime;
    public float MaxWalkSpeed => _maxWalkSpeed;

    public float DashAccelTime => _dashAccelTime;
    public float MaxDashSpeed => _maxDashSpeed; 
    public float RemainDashTime => _remainDashTime;
    public float DashCooltime => _dashCooltime;

    public float JumpPower => _jumpPower;
    public float JumpAccelPower => _jumpAccelPower;
    public float JumpRemainTime => _remainDashTime;
}

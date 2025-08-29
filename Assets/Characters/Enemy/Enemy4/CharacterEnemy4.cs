using UnityEngine;

[CreateAssetMenu(fileName = "CharacterEnemy4", menuName = "Scriptable Objects/CharacterEnemy4")]
public class CharacterEnemy4 : ScriptableObject
{
    private float _bulletSpeed = 5f;

    public float BulletSpeed => _bulletSpeed;
}

using UnityEngine;

[CreateAssetMenu(fileName = "MonsterBullet", menuName = "Scriptable Objects/MonsterBullet")]
public class MonsterBullet : ScriptableObject
{
    public GameObject projectile;
    public float speed = 20f;
    public float standbyTime = 0.2f;
}

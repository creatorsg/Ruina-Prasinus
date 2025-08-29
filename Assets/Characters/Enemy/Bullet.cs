using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Scriptable Objects/Bullet")]
public class Bullet : ScriptableObject
{
    public GameObject projectile;
    public float speed = 20f;
    public float standbyTime = 0.2f;
}

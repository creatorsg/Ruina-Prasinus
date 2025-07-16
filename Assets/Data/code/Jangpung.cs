using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/JangpungSettings", fileName = "Jangpung")]
public class Jangpung : ScriptableObject
{
    public GameObject projectilePrefab;
    public float speed = 20f;
    public float standbyTime = 0.2f;
}

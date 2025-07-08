using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "Scriptable Objects/Map")]
public class Map : ScriptableObject
{
    public float enemy_num = 20;
    public bool open_portal = false;
}

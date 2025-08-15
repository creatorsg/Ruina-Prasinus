using UnityEngine;

[CreateAssetMenu(fileName = "explosion", menuName = "Scriptable Objects/explosion")]
public class explosion : ScriptableObject
{
    public GameObject explosionPrefab;
    private float _explodeTimer;
    private Vector3 _finalScale = new Vector3(5f,0f,5f); 

    public float ExplodeTimer => _explodeTimer;
    public Vector3 FinalScale => _finalScale;
}

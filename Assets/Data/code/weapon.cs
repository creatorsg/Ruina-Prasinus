using UnityEngine;

[CreateAssetMenu(menuName = "Game/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public Sprite icon;
    public CombinationPoint[] combinationPoints;
}

[System.Serializable]
public struct CombinationPoint
{
    public string pointID;  
    public Vector2 localPos; 
}

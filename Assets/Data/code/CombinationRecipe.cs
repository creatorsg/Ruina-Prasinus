using UnityEngine;

[CreateAssetMenu(menuName = "Game/CombinationRecipe2")]
public class CombinationRecipe : ScriptableObject
{
    public WeaponData weaponA;
    public WeaponData weaponB;
    public string combinationPoint;     
    public WeaponData resultWeapon;
}

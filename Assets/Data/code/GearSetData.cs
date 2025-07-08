using UnityEngine;

[CreateAssetMenu(menuName = "Game/GearSet")]
public class GearSetData : ScriptableObject
{
    public WeaponData leftSlot;
    public WeaponData rightSlot;
}

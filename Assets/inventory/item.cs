using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "Scriptable Objects/item")]
public class Item : ScriptableObject
{
    public string id;       
    public string itemName;
    public Sprite icon;
    public int maxStack = 99;
}

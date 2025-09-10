using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveCenter
{
    public Vector3 playerPosition;
    public int playerHealth;
    public List<string> inventoryItems;

    public SaveCenter()
    {
        this.playerPosition = Vector3.zero;
        this.playerHealth = 100;
        this.inventoryItems = new List<string>();
    }
}

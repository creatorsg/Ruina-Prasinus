using UnityEngine;
using static UnityEditor.Progress;

public class Slot
{
    public Item Item { get; private set; }
    public int Amount { get; private set; }

    public Slot(Item item, int amount)
    {
        Item = item;
        Amount = amount;
    }

    public bool AddAmount(int amount)
    {
        if (Item == null) return false;
        int newAmount = Amount + amount;
        if (newAmount <= Item.maxStack)
        {
            Amount = newAmount;
            return true;
        }
        return false;
    }

    public void SetItem(Item item, int amount)
    {
        Item = item;
        Amount = amount;
    }

    public void Clear()
    {
        Item = null;
        Amount = 0;
    }
}

using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public int width = 5;
    public int height = 4;

    [HideInInspector]
    public List<Slot> slots;

    public event Action OnInventoryChanged = delegate { };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSlots();
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSlots()
    {
        slots = new List<Slot>(width * height);
        for (int i = 0; i < width * height; i++)
            slots.Add(new Slot(null, 0));
    }

    public bool AddItem(Item item, int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].Item == item && slots[i].Amount < item.maxStack)
            {
                int canAdd = Mathf.Min(amount, item.maxStack - slots[i].Amount);
                slots[i].AddAmount(canAdd);
                amount -= canAdd;
                if (amount <= 0)
                {
                    OnInventoryChanged();
                    return true;
                }
            }
        }

        for (int i = 0; i < slots.Count && amount > 0; i++)
        {
            if (slots[i].Item == null)
            {
                int add = Mathf.Min(amount, item.maxStack);
                slots[i].SetItem(item, add);
                amount -= add;
            }
        }
        OnInventoryChanged();
        return amount == 0;
    }

    public void SwapSlots(int indexA, int indexB)
    {
        var tmp = slots[indexA];
        slots[indexA] = slots[indexB];
        slots[indexB] = tmp;
        OnInventoryChanged();
    }

    [Serializable]
    private class SlotData { public string itemId; public int amount; }
    [Serializable]
    private class InventoryData { public SlotData[] slots; }

    public void SaveInventory()
    {
        InventoryData data = new InventoryData()
        {
            slots = slots.Select(s => new SlotData
            {
                itemId = s.Item != null ? s.Item.id : "",
                amount = s.Amount
            }).ToArray()
        };
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("Inventory", json);
        PlayerPrefs.Save();
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("Inventory")) return;
        string json = PlayerPrefs.GetString("Inventory");
        InventoryData data = JsonUtility.FromJson<InventoryData>(json);

        for (int i = 0; i < slots.Count && i < data.slots.Length; i++)
        {
            var sd = data.slots[i];
            if (!string.IsNullOrEmpty(sd.itemId))
            {
                Item it = Resources.Load<Item>($"Items/{sd.itemId}");
                slots[i].SetItem(it, sd.amount);
            }
            else
            {
                slots[i].Clear();
            }
        }
        OnInventoryChanged();
    }
}
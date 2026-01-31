using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public event Action<string, int> OnInventoryChanged;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void AddItem(string itemName, int amount = 1)
    {
        var newAmount = 0;
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
            newAmount = inventory[itemName];
        }
        else
        {
            inventory.Add(itemName, amount);
            newAmount = amount;
        }
        Debug.Log($"Collected: {itemName}. Total: {inventory[itemName]}");
        OnInventoryChanged?.Invoke(itemName, newAmount);
    }

    public void RemoveItem(string itemName, int amount = 1)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] -= amount;
            if (inventory[itemName] < 0)
                inventory[itemName] = 0;
            Debug.Log($"Removed: {itemName}. Total: {inventory[itemName]}");
            OnInventoryChanged?.Invoke(itemName, inventory[itemName]);
        }
    }

    public int GetItemCount(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            return inventory[itemName];
        }
        return 0;
    }
}
using UnityEngine;
using TMPro; // Needed for TextMeshPro

public class ItemCounterUI : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Which item ID should this text display?")]
    public string itemID = "MAIN_COLLECTIBLE"; 
    public TextMeshProUGUI textComp;

    void Awake()
    {
        UpdateText(itemID, InventoryManager.Instance.GetItemCount(itemID));
    }

    void OnEnable()
    {
        // Subscribe: "Hey Manager, let me know when inventory changes!"
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged += UpdateText;
    }

    void OnDisable()
    {
        // Unsubscribe: Important to prevent errors when scene changes
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= UpdateText;
    }

    void UpdateText(string changedItemID, int newAmount)
    {
        if (changedItemID != itemID) return;
        textComp.text = newAmount.ToString();
    }
}
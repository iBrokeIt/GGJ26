using UnityEngine;
using TMPro; // Needed for TextMeshPro

public class ItemCounterUI : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Which item ID should this text display?")]
    public string itemID = "MAIN_COLLECTIBLE"; 
    public TextMeshProUGUI textComp;

    void Start()
    {
        UpdateText(itemID, InventoryManager.Instance.GetItemCount(itemID));
    }

    void OnEnable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged += UpdateText;
    }

    void OnDisable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= UpdateText;
    }

    void UpdateText(string changedItemID, int newAmount)
    {
        if (changedItemID != itemID) return;
        textComp.text = newAmount.ToString();
    }
}
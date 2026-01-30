using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Tooltip("The unique ID for this item (e.g., 'GoldCoin', 'BlueGem')")]
    public string itemID;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(itemID);
            Destroy(gameObject);
        }
    }
}
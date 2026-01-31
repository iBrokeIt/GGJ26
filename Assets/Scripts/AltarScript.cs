using UnityEngine;

public class AltarScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int count = InventoryManager.Instance.GetItemCount("MAIN_COLLECTIBLE");
            Debug.Log("Player reached the altar! AltarKey count: " + count);
            if (count >= 3)
            {
                Debug.Log("Altar activated!");
                // Add your altar activation logic here
            }
        }
    }
}

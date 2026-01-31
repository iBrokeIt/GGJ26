using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltarScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int collectibleCount = 0;
    public GameObject player;
    private bool playerInRange = false;
    List<GameObject> altarCollectibles = new List<GameObject>();
    public List<AudioClip> collectibleSounds;
    public AudioClip finishSound;
    public float interactionCooldown = 1f;
    float lastInteractionTime = 0f;
    void Start()
    {
        PlayerContoller playerController = player.GetComponent<PlayerContoller>();
        playerController.interactAction.action.started += OnPlayerInteract;
        for (int i = 1; i <= 3; i++)
        {
            GameObject collectibleVisual = transform.Find("Orb" + i).gameObject;
            collectibleVisual.SetActive(false);
            altarCollectibles.Add(collectibleVisual);
        }
        if (collectibleSounds.Count != altarCollectibles.Count)
        {
            Debug.LogWarning("Mismatch between number of collectible sounds and altar collectibles.");
        }
    }

    void OnDisable()
    {
        if (player == null) return;
        PlayerContoller playerController = player.GetComponent<PlayerContoller>();
        playerController.interactAction.action.started -= OnPlayerInteract;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered altar area");
            playerInRange = true;
        }
    }

    void OnPlayerInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Player interacted with the altar!");
        if (!context.started || !playerInRange) return;
        if (Time.time - lastInteractionTime < interactionCooldown) return;
        lastInteractionTime = Time.time;
        int count = InventoryManager.Instance.GetItemCount("MAIN_COLLECTIBLE");
        Debug.Log($"Player has {count} collectibles."); 
        if (count > 0)
        {
            altarCollectibles[collectibleCount].SetActive(true);
            AudioManager.Instance.PlaySFX(collectibleSounds[collectibleCount]);
            InventoryManager.Instance.RemoveItem("MAIN_COLLECTIBLE", 1);
            collectibleCount += 1;
            // Show additional collectible visuals on the altar here
        }
        else {
            Debug.Log("No collectibles to place on the altar.");
            return;
        }
        if (collectibleCount >= 3)
        {
            // Maybe add some sound or visual effect here
            Debug.Log("Altar activated!");
            Invoke("PlayFinishSequence", interactionCooldown);
        }
    }

    void PlayFinishSequence()
    {
        Debug.Log("Playing altar finish sequence!");
        AudioManager.Instance.PlaySFX(finishSound);
        // Implement the finish sequence logic here
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited altar area");
            playerInRange = false;
            
        }
    }
}

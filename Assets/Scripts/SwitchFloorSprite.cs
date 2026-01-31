using System.Collections.Generic;
using UnityEngine;

public class FloorUpdater : MonoBehaviour
{

    public SpriteRenderer floorRenderer;
    public SpriteRenderer groundRenderer;
    public List<Sprite> floorSprites;
    public List<Sprite> groundSprites;

    // Subscribe to the global change event
    void OnEnable() => DimensionSwitcher.OnWorldChanged += UpdateSprite;
    void OnDisable() => DimensionSwitcher.OnWorldChanged -= UpdateSprite;

    void Start()
    {
        if(!floorRenderer)
        {
            Debug.LogWarning("Floor Renderer is not assigned in FloorUpdater.");
        }
        if(!groundRenderer)
        {
            Debug.LogWarning("Ground Renderer is not assigned in FloorUpdater.");
        }
    }

    void UpdateSprite(int newSpriteIndex)
    {
        if(floorSprites != null && floorSprites[newSpriteIndex] != null)
        {
            floorRenderer.sprite = floorSprites[newSpriteIndex];
        }
        else
        {
            Debug.LogWarning("Floor sprite not found for index: " + newSpriteIndex);
        }

        if(groundSprites != null && groundSprites[newSpriteIndex] != null)
        {
            groundRenderer.sprite = groundSprites[newSpriteIndex];
        }
        else
        {
            Debug.LogWarning("Ground sprite not found for index: " + newSpriteIndex);
        }
    }
}
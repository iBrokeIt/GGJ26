using System.Collections.Generic;
using UnityEngine;

public class FloorUpdater : MonoBehaviour
{

    public SpriteRenderer floorRenderer;
    public List<Sprite> floorSprites;

    // Subscribe to the global change event
    void OnEnable() => DimensionSwitcher.OnWorldChanged += UpdateSprite;
    void OnDisable() => DimensionSwitcher.OnWorldChanged -= UpdateSprite;

    void UpdateSprite(int newSpriteIndex)
    {
        if(floorSprites[newSpriteIndex] != null)
        {
            
        floorRenderer.sprite = floorSprites[newSpriteIndex];
        }
        else
        {
            Debug.LogWarning("Floor sprite not found for index: " + newSpriteIndex);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites;

    // Subscribe to the global change event
    void OnEnable() => DimensionSwitcher.OnWorldChanged += UpdateSprite;
    void OnDisable() => DimensionSwitcher.OnWorldChanged -= UpdateSprite;

    void Start()
    {
        if(!spriteRenderer)
        {
            Debug.LogWarning("Sprite Renderer is not assigned in SpriteSwitcher.");
        }
    }

    void UpdateSprite(int newSpriteIndex)
    {
        if(spriteRenderer && sprites[newSpriteIndex] != null)
        {
            spriteRenderer.sprite = sprites[newSpriteIndex];
        }
        else
        {
            Debug.LogWarning("Sprite not found for index: " + newSpriteIndex);
            spriteRenderer.sprite = null;
        }
    }
}
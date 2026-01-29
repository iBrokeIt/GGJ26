using System.Collections.Generic;
using UnityEngine;

public class DimensionSwitcher : MonoBehaviour
{
    private static Color[] WORLD_COLORS = {
    Color.cyan,
    Color.magenta,
    Color.yellow
};

    [System.Serializable]
    public class Dimension
    {
        public string name;
        [Tooltip("Select ONLY the specific layer for this world (e.g., World_A)")]
        public LayerMask uniqueLayerMask; 
        
        [HideInInspector] public int calculatedLayerIndex;
    }

    [Header("Configuration")]
    public Camera mainCam;
    public LayerMask playerLayerMask; 
    public LayerMask commonLayers; 

    [Header("Worlds")]
    public List<Dimension> dimensions;

    private int playerLayerIndex;
    private int currentDimensionIndex = 0;

    void Awake()
    {
        InitializeDimensions();
        ApplyDimension(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SwitchToNextDimension();
        }
    }

    private void InitializeDimensions()
    {
        // 1. Convert Player Mask to Index
        playerLayerIndex = GetLayerIndexFromMask(playerLayerMask);
        
        if (playerLayerIndex == -1) 
            Debug.LogError("Please select exactly ONE layer for the Player Layer Mask.");

        // 2. Setup Dimensions
        foreach (var dim in dimensions)
        {
            dim.calculatedLayerIndex = GetLayerIndexFromMask(dim.uniqueLayerMask);
            
            if (dim.calculatedLayerIndex == -1) 
                Debug.LogError($"Dimension '{dim.name}' has invalid mask! Select exactly ONE layer.");
            
            DisablePhysicsForLayer(dim.calculatedLayerIndex);
        }
    }

    private void EnablePhysicsForLayer(int layerIndex)
    {
        if(layerIndex >= 0)
            Physics2D.IgnoreLayerCollision(playerLayerIndex, layerIndex, false);
    }

    private void DisablePhysicsForLayer(int layerIndex)
    {
        if(layerIndex >= 0)
            Physics2D.IgnoreLayerCollision(playerLayerIndex, layerIndex, true);
    }

    private void SwitchToNextDimension()
    {
        var nextDimensionIndex = (currentDimensionIndex + 1) % dimensions.Count;
        
        RemoveDimension(currentDimensionIndex);
        ApplyDimension(nextDimensionIndex);
        
        currentDimensionIndex = nextDimensionIndex;
    }
    
    private void RemoveDimension(int index)
    {
        var dim = dimensions[index];
        DisablePhysicsForLayer(dim.calculatedLayerIndex);
    }

    private void ApplyDimension(int index)
    {
        var dim = dimensions[index];
        Debug.Log($"Switching to {dim.name}");

        mainCam.cullingMask = commonLayers | dim.uniqueLayerMask | playerLayerMask;
        mainCam.backgroundColor = WORLD_COLORS[index];
        EnablePhysicsForLayer(dim.calculatedLayerIndex);
    }

    private int GetLayerIndexFromMask(LayerMask mask)
    {
        int value = mask.value;
        if (value == 0) return -1;
        
        for (int i = 0; i < 32; i++)
        {
            if ((value & (1 << i)) != 0)
            {
                return i; 
            }
        }
        return -1;
    }
}
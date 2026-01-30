using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        public InputActionReference switchAction; 
        
        [HideInInspector] public System.Action<InputAction.CallbackContext> inputHandler;
        [HideInInspector] public int calculatedLayerIndex;
    }

    [Header("Singleton")]
    public static DimensionSwitcher Instance { get; private set; }

    [Header("Configuration")]
    public Camera mainCam;
    public LayerMask playerLayerMask; 
    public LayerMask commonLayers; 

    [Header("Worlds")]
    public List<Dimension> dimensions;

    private bool devMode = false;
    private int playerLayerIndex;
    private int currentDimensionIndex = 0;

    void Awake()
    {
        // Singleton Setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
            return;
        }
        InitializeDimensions();
        ApplyDimension(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DevModeToggleAllLayersOn();
        }
    }

    void OnEnable()
    {
        for(int i = 0; i < dimensions.Count; i++)
        {
            var dim = dimensions[i];
            var scopedIndex = i;
            if (dim.switchAction != null)
            {
                dim.switchAction.action.Enable();
                dim.inputHandler = ctx => SwitchToDimension(scopedIndex);
                dim.switchAction.action.performed += dim.inputHandler;
                Debug.Log($"Subscribed to switch action for dimension: {dim.name} with Key {dim.switchAction.action.bindings[0].effectivePath}");
            }
            else
            {
                Debug.LogError($"No switch action assigned for dimension: {dim.name}");
            }
        }
    }

    void OnDisable()
{
    foreach (var dim in dimensions)
    {
        if (dim.switchAction != null && dim.inputHandler != null)
        {
            // 3. Unsubscribe using the EXACT same handler we saved
            dim.switchAction.action.performed -= dim.inputHandler;
            dim.inputHandler = null; // Clean up

            dim.switchAction.action.Disable();
        }
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

    public void SwitchToDimension(int nextDimensionIndex)
    {        
        if (nextDimensionIndex == currentDimensionIndex) return;
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

    private void DevModeToggleAllLayersOn()
    {
        devMode = !devMode;
        if (devMode)
        {
            for (int i = 0; i < dimensions.Count; i++)
            {
                ApplyDimension(i);
            }
            mainCam.cullingMask = ~0;

        }
        else
        {
            for(int i = 0; i < dimensions.Count; i++)
            {
                RemoveDimension(i);
            }
            ApplyDimension(currentDimensionIndex);
        }
       
    }
}
using System;
using UnityEngine;

public class MaskSwitcer : MonoBehaviour
{
    public LayerMask WORLD_A_LAYERS;
    public LayerMask WORLD_B_LAYERS;
    public LayerMask WORLD_C_LAYERS;
    
    private int playerLayer = 0;
    private int worldALayer = 6;
    private int worldBLayer = 7;
    private int worldCLayer = 8;
    public Camera mainCam;

    private LayerMask[] cameraLayers;
    private int[] physicsLayers;
    private int currentLayerIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cameraLayers = new LayerMask[] { WORLD_A_LAYERS, WORLD_B_LAYERS, WORLD_C_LAYERS };
        physicsLayers = new int[] { worldALayer, worldBLayer, worldCLayer };
        SetCameraMask(cameraLayers[0]);
        EnablePhysicsForLayer(physicsLayers[0]);
        DisablePhysicsForLayer(physicsLayers[1]);
        DisablePhysicsForLayer(physicsLayers[2]);  
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Or whatever button you prefer
        {
            SwitchToNextLayer();
        }
    }
    private void SetCameraMask(LayerMask mask)
    {
        mainCam.cullingMask = mask;
    }

    private void EnablePhysicsForLayer(LayerMask layer)
    {
        Physics2D.IgnoreLayerCollision(playerLayer, layer, false);
    }

    private void DisablePhysicsForLayer(LayerMask layer)
    {
        Physics2D.IgnoreLayerCollision(playerLayer, layer, true);
    }

    private void SwitchToNextLayer()
    {
        Debug.Log("Switching Layers");
        Debug.Log("Current Layer Index: " + currentLayerIndex);
        var nextLayerIndex = (currentLayerIndex + 1) % cameraLayers.Length;
         
        SetCameraMask(cameraLayers[nextLayerIndex]);
        DisablePhysicsForLayer(physicsLayers[currentLayerIndex]);
        EnablePhysicsForLayer(physicsLayers[nextLayerIndex]);
        currentLayerIndex = nextLayerIndex;
    }
}

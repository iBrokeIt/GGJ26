using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    void Start()
    {
        // Ensure the player starts in the Physical World (dimension 0)
        if (DimensionSwitcher.Instance != null)
        {
            DimensionSwitcher.Instance.SwitchToDimension(0);
        }
    }
}

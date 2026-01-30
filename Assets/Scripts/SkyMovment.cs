using UnityEngine;

public class SkyMovment : MonoBehaviour
{

    public Transform camTransform;
    public float scrollSpeed = 0.5f;
    private float offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (camTransform == null)
            camTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (camTransform == null) return;

        offset += scrollSpeed * Time.deltaTime;

        transform.position = new Vector3(camTransform.position.x + offset, camTransform.position.y, transform.position.z);
    }
}

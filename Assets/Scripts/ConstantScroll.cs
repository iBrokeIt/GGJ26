using UnityEngine;

public class ConstantScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.5f; 
    private float length, startPos;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
     
        transform.position += Vector3.right * scrollSpeed * Time.deltaTime;

        if (transform.position.x > startPos + length)
        {
            transform.position = new Vector3(startPos, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < startPos - length)
        {
            transform.position = new Vector3(startPos, transform.position.y, transform.position.z);
        }
    }
}
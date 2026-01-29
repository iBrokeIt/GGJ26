using UnityEngine;

public class ElevatorLogic : MonoBehaviour
{

    public Transform posA, posB;
    public float speed = 2f;
    Vector3 targetPos;

    void Start() => targetPos = posB.position;

    void FixedUpdate() 
    {
        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            targetPos = (targetPos == posA.position) ? posB.position : posA.position;
        }

   
        Vector2 newPos = Vector2.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
        GetComponent<Rigidbody2D>().MovePosition(newPos);
    }
}

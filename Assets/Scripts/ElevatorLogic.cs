using UnityEngine;

public class ElevatorLogic : MonoBehaviour
{

    public Transform posA, posB;
    public float speed = 2f;
    
    private Rigidbody2D rb;
    Vector3 targetPos;           

    void Start()
    {
        targetPos = posB.position;
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
       
        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            targetPos = (targetPos == posA.position) ? posB.position : posA.position;
        }
        rb.linearVelocity = (targetPos - transform.position).normalized * speed;
    }



  

}

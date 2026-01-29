using UnityEngine;

public class ElevatorLogic : MonoBehaviour
{

    public Transform posA, posB;
    public float speed = 2f;     
    Vector3 targetPos;           

    void Start()
    {
        targetPos = posB.position;
    }

    void Update()
    {
       
        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            targetPos = (targetPos == posA.position) ? posB.position : posA.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }



  

}

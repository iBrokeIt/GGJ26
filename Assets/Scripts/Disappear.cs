using UnityEngine;

public class Disappear : MonoBehaviour
{
    public float timeToDisappear = 2.0f;
    //public float 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToDisappear > 0)
        {
            timeToDisappear -= Time.deltaTime;
            if (timeToDisappear <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

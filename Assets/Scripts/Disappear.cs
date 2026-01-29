using Unity.VisualScripting;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    public float timeToDisappear = 2.0f;
    public float timeToAppear = 5.0f;
    public bool startDisappeared = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

 private System.Collections.IEnumerator DisappearAndAppear()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(timeToDisappear);

        // Make the object disappear
        gameObject.SetActive(false);

        // Wait for the specified time
        yield return new WaitForSeconds(timeToAppear);

        // Make the object appear again
        gameObject.SetActive(true);
 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            startDisappeared = true;
            StartCoroutine(DisappearAndAppear());
            startDisappeared = false;
        }
    }
}


using Unity.VisualScripting;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    public float timeToDisappear = 2.0f;
    public float timeToAppear = 5.0f;


    private bool startDisappeared = false;

    private SpriteRenderer spriteRenderer;
    private Collider2D collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
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
        spriteRenderer.enabled = false;
        collider.enabled = false;

        // Wait for the specified time
        yield return new WaitForSeconds(timeToAppear);

        // Make the object appear again
        spriteRenderer.enabled = true;
        collider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !startDisappeared)
        {
            startDisappeared = true;
            StartCoroutine(DisappearAndAppear());
            startDisappeared = false;
        }
    }
}


using Unity.VisualScripting;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    [Header("Settings")]
    public float timeToDisappear = 2.0f;
    public float timeToAppear = 5.0f;

    [Header("SFX")]
    public AudioClip disappearSound;

    private bool startDisappeared = false;

    private SpriteRenderer spriteRenderer;
    private Collider2D myCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
    }

 private System.Collections.IEnumerator DisappearAndAppear()
    {
        // Wait for the specified time
        yield return new WaitForSeconds(timeToDisappear);

        // Make the object disappear
        AudioManager.Instance.PlayRandomizedSFX(disappearSound);
        spriteRenderer.enabled = false;
        myCollider.enabled = false;

        // Wait for the specified time
        yield return new WaitForSeconds(timeToAppear);

        // Make the object appear again
        spriteRenderer.enabled = true;
        myCollider.enabled = true;
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


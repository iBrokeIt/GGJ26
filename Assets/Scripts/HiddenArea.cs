using UnityEngine;

public class ReduceOpacity : MonoBehaviour
{
    public float lowOpacity = 0.5f;
    public float normalOpacity = 1f;
    public float opactityChangeTime = 0.5f;
    public float timer = 0f;
    public bool isReducing = false;
    SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReducing)
        {
            timer += Time.deltaTime;
            if (timer >= opactityChangeTime)
            {
                timer = opactityChangeTime;
            }
        }
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
            }
        }
        float t = timer / opactityChangeTime;
        Color c = spriteRenderer.color;
        c.a = Mathf.Lerp(normalOpacity, lowOpacity, t);
        spriteRenderer.color = c;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isReducing = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isReducing = false;
        }
    }
}

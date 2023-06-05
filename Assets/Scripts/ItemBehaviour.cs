using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public float fallSpeed = 5f;
    public Sprite[] itemSprites;
    public SpriteRenderer itemSpriteRenderer;

    private void Start()
    {
        // Randomly choose a sprite for the item
        itemSpriteRenderer.sprite = itemSprites[Random.Range(0, itemSprites.Length)];

        // add a random rotation
        transform.Rotate(0f, 0f, Random.Range(-30f, 30f));

        // add small random impulse
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-5f, 5f), 0f), ForceMode2D.Impulse);
    }

    private void Update()
    {
        // Move the item downwards
        // transform.Translate(fallSpeed * Time.deltaTime * Vector3.down);

        // Check if the item has moved off the screen
        if (transform.position.y < -8f)
        {
            Destroy(gameObject);
        }
    }
}


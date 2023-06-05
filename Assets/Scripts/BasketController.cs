using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameManager gameManager;

    public bool isMoving = true;

    private float initialYPosition;
    private float initialZPosition;

    private void Start()
    {
        initialYPosition = transform.position.y;
        initialZPosition = transform.position.z;
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveBasket();
        }
    }

    private void MoveBasket()
    {
        // Get the current mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Mathf.Clamp(Input.mousePosition.x, 0f, Screen.width), 0f, 0f));
        mousePosition.y = initialYPosition;
        mousePosition.z = initialZPosition;

        // Interpolate the basket's position towards the mouse position
        transform.position = Vector3.Lerp(transform.position, mousePosition, 0.03f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if a good item has entered the basket
        if (other.CompareTag("GoodItem"))
        {
            // Increase score or perform other actions
            // based on collecting a good item
            Debug.Log("Good item collected!");
            scoreManager.IncreaseScore(1);
        }
        // Check if a bad item has entered the basket
        else if (other.CompareTag("BadItem"))
        {
            // Decrease score or perform other actions
            // based on collecting a bad item
            Debug.Log("Bad item collected!");
            gameManager.GameOver();
        }

        // Destroy the item regardless of its type
        Destroy(other.gameObject);
    }
}

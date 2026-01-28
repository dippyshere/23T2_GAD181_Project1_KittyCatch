using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasketController : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameManager gameManager;
    public GameObject collectPrefab;
    public Transform smokeSpawn;

    public bool isMoving = true;

    private float initialYPosition;
    private float initialZPosition;
    private Camera mainCamera;

    private void Start()
    {
        initialYPosition = transform.position.y;
        initialZPosition = transform.position.z;
        mainCamera = Camera.main;
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
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(Mathf.Clamp(Mouse.current.position.value.x, 0f, Screen.width), 0f, 0f));
        mousePosition.y = initialYPosition;
        mousePosition.z = initialZPosition;

        // Interpolate the basket's position towards the mouse position
        transform.position = Vector3.Lerp(transform.position, mousePosition, 0.065f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if a good item has entered the basket
        if (other.CompareTag("GoodItem"))
        {
            // Increase score or perform other actions
            // based on collecting a good item
            //Debug.Log("Good item collected!");
            scoreManager.IncreaseScore(1);
            Instantiate(collectPrefab, smokeSpawn);
        }
        // Check if a bad item has entered the basket
        else if (other.CompareTag("BadItem"))
        {
            // Decrease score or perform other actions
            // based on collecting a bad item
            //Debug.Log("Bad item collected!");
            gameManager.GameOver();
            Instantiate(gameManager.explosionPrefab, other.transform.position, Quaternion.identity);
        }

        // Destroy the item regardless of its type
        Destroy(other.gameObject);
    }
}

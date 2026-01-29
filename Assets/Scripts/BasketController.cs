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
    InputAction moveAction;

    private void Start()
    {
        initialYPosition = transform.position.y;
        initialZPosition = transform.position.z;
        mainCamera = Camera.main;
        moveAction = InputSystem.actions.FindAction("Move");
    }

    public void Update()
    {
        MoveBasket();
    }

    void MoveBasket()
    {
        if (!isMoving)
        {
            return;
        }

        Vector3 touchPosition = mainCamera.ScreenToWorldPoint(new Vector3(Mathf.Clamp(moveAction.ReadValue<Vector2>().x, 0f, Screen.width), 0, 0));
        touchPosition.y = -3.5f;
        touchPosition.z = 0;
        transform.position = Vector3.Lerp(transform.position, touchPosition, 0.065f);;
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

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
    public PlayRandomSound collectSound;
    public PlayRandomSound gameOverSound;

    public bool isMoving = true;

    private float initialYPosition;
    private float initialZPosition;
    private Camera mainCamera;
    InputAction moveAction;
    InputAction controlAction;
    Vector2 lastMousePosition;
    Vector2 lastControlPosition;
    bool isControlling;

    private void Start()
    {
        initialYPosition = transform.position.y;
        initialZPosition = transform.position.z;
        mainCamera = Camera.main;
        moveAction = InputSystem.actions.FindAction("Move");
        controlAction = InputSystem.actions.FindAction("Control");
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

        Vector2 currentMousePosition = moveAction.ReadValue<Vector2>();
        Vector2 currentControlPosition = controlAction.ReadValue<Vector2>();
        Vector2 touchPosition = transform.position;

        if (isControlling)
        {
            if (currentMousePosition != lastMousePosition)
            {
                isControlling = false;
            }
        }
        else
        {
            if (currentControlPosition != lastControlPosition)
            {
                isControlling = true;
            }
        }
        lastMousePosition = currentMousePosition;
        lastControlPosition = currentControlPosition;

        if (isControlling)
        {
            float screenX = (currentControlPosition.x + 1f) / 2f * Screen.width;
            touchPosition = mainCamera.ScreenToWorldPoint(new Vector3(Mathf.Clamp(screenX, 0f, Screen.width), 0, 0));
        }
        else
        {
            touchPosition = mainCamera.ScreenToWorldPoint(new Vector3(Mathf.Clamp(moveAction.ReadValue<Vector2>().x, 0f, Screen.width), 0, 0));
        }

        touchPosition.y = -3.5f;
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
            collectSound.PlayRandomAudioClip();
            // instantiate the smoke prefab asynchronously parented to the smoke spawn point and set the local transform to zero after instantiating
            AsyncInstantiateOperation<GameObject> operation = InstantiateAsync(collectPrefab, smokeSpawn.position, Quaternion.identity);
            operation.completed += (result) =>
            {
                operation.Result[0].transform.SetParent(smokeSpawn);
                operation.Result[0].transform.localPosition = Vector3.zero;
            };
        }
        // Check if a bad item has entered the basket
        else if (other.CompareTag("BadItem"))
        {
            // Decrease score or perform other actions
            // based on collecting a bad item
            //Debug.Log("Bad item collected!");
            gameManager.GameOver();
            gameOverSound.PlayRandomAudioClip();
            InstantiateAsync(gameManager.explosionPrefab, other.transform.position, Quaternion.identity);
        }

        // Destroy the item regardless of its type
        Destroy(other.gameObject);
    }
}

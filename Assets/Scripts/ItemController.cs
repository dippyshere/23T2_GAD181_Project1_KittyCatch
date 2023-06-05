using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject goodItemPrefab;
    public GameObject badItemPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 1f;

    private float timer;

    private void Start()
    {
        timer = spawnInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Randomly choose between good and bad items
            GameObject itemPrefab = Random.Range(0, 2) == 0 ? goodItemPrefab : badItemPrefab;

            spawnPoint.position = new Vector3(Random.Range(-4f, 4f), spawnPoint.position.y, spawnPoint.position.z);

            // Instantiate the item at the spawn point
            GameObject item = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);

            // Reset the timer for the next spawn
            timer = spawnInterval;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    private PlayerController playerScript; // Reference to the Player script

    void Start()
    {
        // Find the player in the scene and get a reference to the Player script
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        if (playerScript == null)
        {
            Debug.LogError("Player script not found on the player object.");
        }
    }

    void Update()
    {
        // Check if Fire1 button is pressed and batteryAmount is greater than 0
        if (Input.GetButtonDown("Fire1") && playerScript != null && playerScript.batteryAmount > 0)
        {
            Shoot();
            // Optional: Reduce battery amount by 1 each time the weapon is fired
            playerScript.batteryAmount -= 1;
        }
    }

    void Shoot()
    {
        // Instantiate bullet from firePoint's position and rotation
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}

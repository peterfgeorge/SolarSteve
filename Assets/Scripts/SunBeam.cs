using System.Collections;
using UnityEngine;

public class Sunbeam : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool alreadyFaded = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("Sunbeam prefab must have a SpriteRenderer component.");
        }
        else
        {
            StartFading(0.5f, 1.5f); // Start fading when the sunbeam spawns
        }
    }

    // Method to start fading the sunbeam in and out
    public void StartFading(float fadeDuration, float displayDuration)
    {
        StartCoroutine(FadeInAndOut(fadeDuration, displayDuration));
    }

    // Coroutine for fading in and out
    private IEnumerator FadeInAndOut(float fadeDuration, float displayDuration)
    {
        if (sr == null) yield break;

        // Fade in
        yield return Fade(0f, 1f, fadeDuration);

        // Stay visible for the display duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out after the display time
        if(alreadyFaded != true) {
            yield return Fade(1f, 0f, fadeDuration);
        }

        // Destroy the sunbeam after fade-out
        Destroy(gameObject);
    }

    // Method to fade the sprite from one alpha to another
    private IEnumerator Fade(float fromAlpha, float toAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = sr.color;

        while (elapsed < duration)
        {
            if (sr == null) yield break; // Exit if the sprite renderer is destroyed

            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / duration);
            sr.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure the final alpha value is set
        if (sr != null)
        {
            sr.color = new Color(color.r, color.g, color.b, toAlpha);
        }
    }

    // Handle the player collision to fade the sunbeam out quickly
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop any ongoing fade
            // StopAllCoroutines();

            // Quickly fade out the sunbeam
            StartCoroutine(Fade(1f, 0f, 0.75f));
            alreadyFaded = true;

            // Get the player's script and increment batteryAmount
            PlayerController playerScript = other.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                if(playerScript.batteryAmount < 16) {
                    playerScript.batteryAmount += 1;  // Increase the battery amount by 1
                    Debug.Log("Battery increased! New battery amount: " + playerScript.batteryAmount);
                }
            }
            else
            {
                Debug.LogError("Player script not found on the player object.");
            }
        }
    }

}

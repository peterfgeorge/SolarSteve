using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public Image heartImage;  // Reference to the Image component on the canvas
    public Sprite oneHeart;   // Sprite for 1 Heart level
    public Sprite twoHeart;   // Sprite for 2 Heart levels
    public Sprite threeHeart; // Sprite for 3 Heart levels

    public PlayerController player;  // Reference to the Player script that holds HeartAmount

    void Start()
    {
        // Find the player in the scene if not manually assigned
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        UpdateHeartImage(); // Update the Heart image when the script starts
    }

    void Update()
    {
        // Continuously check and update the Heart image
        UpdateHeartImage();
    }

    // Update the Heart image based on the current Heart amount
    void UpdateHeartImage()
    {
        if (player == null) return; // Make sure player is assigned

        int HeartAmount = player.health;

        if (HeartAmount < 1)
        {
            heartImage.sprite = null;  // Set to empty Heart
        }
        else if (HeartAmount == 1)
        {
            heartImage.sprite = oneHeart;   // Set to one Heart
        }
        else if (HeartAmount == 2)
        {
            heartImage.sprite = twoHeart;   // Set to two batteries
        }
        else if (HeartAmount == 3)
        {
            heartImage.sprite = threeHeart; // Set to three batteries
        }
    }
}

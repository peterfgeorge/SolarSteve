using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    public Image batteryImage;  // Reference to the Image component on the canvas
    public Sprite emptyBattery;  // Sprite for empty battery
    public Sprite oneBattery;   // Sprite for 1 battery level
    public Sprite twoBattery;   // Sprite for 2 battery levels
    public Sprite threeBattery; // Sprite for 3 battery levels
    public Sprite fourBattery;  // Sprite for 4 battery levels

    public PlayerController player;  // Reference to the Player script that holds batteryAmount

    void Start()
    {
        // Find the player in the scene if not manually assigned
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        UpdateBatteryImage(); // Update the battery image when the script starts
    }

    void Update()
    {
        // Continuously check and update the battery image
        UpdateBatteryImage();
    }

    // Update the battery image based on the current battery amount
    void UpdateBatteryImage()
    {
        if (player == null) return; // Make sure player is assigned

        int batteryAmount = player.batteryAmount;

        if (batteryAmount < 1)
        {
            batteryImage.sprite = emptyBattery;  // Set to empty battery
        }
        else if (batteryAmount >= 1 && batteryAmount <= 4)
        {
            batteryImage.sprite = oneBattery;   // Set to one battery
        }
        else if (batteryAmount >= 4 && batteryAmount <= 7)
        {
            batteryImage.sprite = twoBattery;   // Set to two batteries
        }
        else if (batteryAmount >= 7 && batteryAmount <= 11)
        {
            batteryImage.sprite = threeBattery; // Set to three batteries
        }
        else if (batteryAmount >= 12)
        {
            batteryImage.sprite = fourBattery;  // Set to four batteries
        }
    }
}

using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    // Reference to the TextMeshPro UI text object
    public TextMeshProUGUI coinText;

    // Counter for the collected coins
    private int coinCount = 0;

    // Trigger detection for coin collection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            // Prevent triggering more than once
            Destroy(collision.gameObject);

            // Increment the coin counter
            coinCount++;

            // Update the text on screen
            coinText.text = "" + coinCount;
        }
    }

}

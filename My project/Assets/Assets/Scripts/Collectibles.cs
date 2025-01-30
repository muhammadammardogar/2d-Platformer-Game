using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public AudioSource coinSound; // Reference to the AudioSource

    private int coinCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            Coin coin = collision.gameObject.GetComponent<Coin>();
            if (coin != null && !coin.IsCollected)
            {
                coin.MarkCollected(); // Mark the coin as collected

                // Play the coin collection sound
                if (coinSound != null)
                {
                    coinSound.Play();
                }

                // Increment the coin counter
                coinCount++;

                // Update the UI text
                coinText.text = coinCount.ToString();

                // Destroy the coin after a short delay
                Destroy(collision.gameObject, 0.05f);
            }
        }
    }
}

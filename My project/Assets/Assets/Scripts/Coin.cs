using UnityEngine;

public class Coin : MonoBehaviour
{
    public bool IsCollected { get; private set; } = false;

    public void MarkCollected()
    {
        IsCollected = true;
        GetComponent<Collider2D>().enabled = false; // Disable collider to prevent further collection
    }
}

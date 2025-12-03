using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    [Header("Buff Settings")]
    public float speedMultiplier = 2f;  // How much faster the player will go
    public float duration = 3f;         // How long the boost lasts in seconds

    [Header("Optional Visual Feedback")]
    public bool rotatePickup = true;
    public float rotationSpeed = 50f;

    private void Update()
    {
        // Optional: Make the pickup spin for visual appeal
        if (rotatePickup)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Try to get the PlayerMovement component from whatever collided
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            // Apply the speed buff to the player
            player.ApplySpeedBuff(speedMultiplier, duration);

            // Destroy this pickup
            Destroy(gameObject);
        }
    }
}
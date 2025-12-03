using UnityEngine;

public class SpeedBuffVisual : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float pulseSpeed = 2f;
    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.GetColor("_EmissionColor");
    }

    void Update()
    {
        // Rotate the cube
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Pulse glow
        float emission = Mathf.PingPong(Time.time * pulseSpeed, 1.5f) + 0.5f;
        // Adjust the emission intensity to enhance the glow effect
        rend.material.SetColor("_EmissionColor", originalColor * emission * 2.0f);
    }
}

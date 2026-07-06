using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("UI References")]
    public Image stabilityBar;
    public TextMeshProUGUI speedText;

    [Header("Physics Reference")]
    public Rigidbody truckRigidbody; // Drag your GravityTruck here

    void OnEnable()
    {
        // Listen to the physics events from Day 2
        GameEventBus.OnCargoTiltChanged += UpdateStabilityBar;
    }

    void OnDisable()
    {
        GameEventBus.OnCargoTiltChanged -= UpdateStabilityBar;
    }

    void Update()
    {
        // Calculate and display speed every frame
        if (truckRigidbody != null && speedText != null)
        {
            // linearVelocity.magnitude gives meters per second. 
            // Multiplying by 3.6 converts it to KM/H.
            float speed = truckRigidbody.linearVelocity.magnitude * 3.6f;
            
            // "F0" removes decimals (e.g., 60 instead of 60.234)
            speedText.text = speed.ToString("F0") + " KM/H";
        }
    }

    void UpdateStabilityBar(float angle)
    {
        if (stabilityBar == null) return;

        // Calculate fill (0 to 1). 45 degrees is our failure point.
        float fill = Mathf.Abs(angle) / 45f;
        stabilityBar.fillAmount = Mathf.Clamp01(fill);

        // Professional Polish: Smoothly transition from Green to Red
        stabilityBar.color = Color.Lerp(Color.green, Color.red, fill);
    }
}

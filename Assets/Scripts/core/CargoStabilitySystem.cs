using UnityEngine;

// Attach to the Cargo parent GameObject (child of truck chassis).
// Each crate is a child Rigidbody with its own mass.
public class CargoStabilitySystem : MonoBehaviour
{
    [Header("Thresholds")]
    [SerializeField] private float warnTiltAngle   = 25f;
    [SerializeField] private float criticalTiltAngle = 40f;
    [SerializeField] private float lostHeightThreshold = -3f;

    [Header("Crate Root")]
    [SerializeField] private Transform crateRoot;

    private Rigidbody[] _crates;
    private bool        _cargoLost = false;

    void Start()
    {
        // Collect all child Rigidbodies as crates
        _crates = crateRoot.GetComponentsInChildren<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (_cargoLost) return;
        CheckTilt();
        CheckFallenCrates();
    }

    void CheckTilt()
    {
        // Truck's local Z rotation = forward tilt (pitch)
        float pitch = transform.eulerAngles.z;
        // Normalize from 0-360 to -180 to 180
        if (pitch > 180f) pitch -= 360f;
        float absPitch = Mathf.Abs(pitch);

        GameEventBus.FireCargoTiltChanged(absPitch);


        if (absPitch >= criticalTiltAngle)
            TriggerCargoLost();
    }

    void CheckFallenCrates()
    {
        foreach (var crate in _crates)
        {
            if (crate == null) continue;
            // Crate fell below world floor threshold
            if (crate.position.y < lostHeightThreshold)
            {
                TriggerCargoLost();
                return;
            }
        }
    }

    void TriggerCargoLost()
    {
        if (_cargoLost) return;
        _cargoLost = true;
        Debug.Log("[Cargo] LOST — triggering game over");
        GameEventBus.FireCargoLost();
    }

    // Call this when truck reaches the destination
    public void OnDeliveryZoneReached()
    {
        if (!_cargoLost)
            GameEventBus.FireLevelComplete();
    }
}
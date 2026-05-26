using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleController : MonoBehaviour
{
    public VehicleStatsSO stats;

    [Header("Wheel Colliders")]
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    [Header("Wheel Visual Meshes")]
    public Transform meshFL;
    public Transform meshFR;
    public Transform meshRL;
    public Transform meshRR;

    private Rigidbody rb;
    private float currentMotorTorque;
    private float currentSteerAngle;
    private float currentBrakeTorque; // Stores our active braking power

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Apply weights and center of mass from our ScriptableObject asset
        rb.mass = stats.vehicleMass;
        rb.centerOfMass = stats.centerOfMassOffset;

        ConfigureSuspension(wheelFL);
        ConfigureSuspension(wheelFR);
        ConfigureSuspension(wheelRL);
        ConfigureSuspension(wheelRR);
    }

    void Update()
    {
        // Get player inputs from keyboard arrow keys or WASD
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        // Check if the player is holding down the Spacebar
        bool isBraking = Input.GetKey(KeyCode.Space);

        // If holding Space, use the high brakeTorque value from our asset card. Otherwise, use 0.
        currentBrakeTorque = isBraking ? stats.brakeTorque : 0f;

        // Calculate driving forces based on your stats asset
        currentMotorTorque = moveInput * stats.motorTorque;
        currentSteerAngle = steerInput * stats.steerAngle;

        // Sync visual wheel meshes to physical wheel colliders so they rotate
        UpdateWheelMesh(wheelFL, meshFL);
        UpdateWheelMesh(wheelFR, meshFR);
        UpdateWheelMesh(wheelRL, meshRL);
        UpdateWheelMesh(wheelRR, meshRR);
    }

    void FixedUpdate()
    {
        // Turn the front wheels
        wheelFL.steerAngle = currentSteerAngle;
        wheelFR.steerAngle = currentSteerAngle;

        // Apply our brake torque to all 4 wheels
        wheelFL.brakeTorque = currentBrakeTorque;
        wheelFR.brakeTorque = currentBrakeTorque;
        wheelRL.brakeTorque = currentBrakeTorque;
        wheelRR.brakeTorque = currentBrakeTorque;

        // Drive the rear wheels (Rear-Wheel Drive) only if we aren't exceeding max speed
        if (rb.linearVelocity.magnitude < stats.maxSpeed)
        {
            wheelRL.motorTorque = currentMotorTorque;
            wheelRR.motorTorque = currentMotorTorque;
        }
        else
        {
            wheelRL.motorTorque = 0;
            wheelRR.motorTorque = 0;
        }
    }

    void ConfigureSuspension(WheelCollider wheel)
    {
        wheel.suspensionDistance = stats.suspensionDist;
        JointSpring spring = wheel.suspensionSpring;
        spring.spring = stats.springForce;
        spring.damper = stats.damperForce;
        wheel.suspensionSpring = spring;
    }

    void UpdateWheelMesh(WheelCollider collider, Transform mesh)
    {
        if (mesh == null) return;
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        mesh.position = position;
        mesh.rotation = rotation;
    }
}

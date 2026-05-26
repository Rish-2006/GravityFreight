using UnityEngine;

[CreateAssetMenu(fileName = "VehicleStats", menuName = "GravityFreight/VehicleStats")]
public class VehicleStatsSO : ScriptableObject
{
    public float motorTorque = 1500f;
    public float brakeTorque = 3000f;
    public float maxSpeed = 25f;
    public float steerAngle = 25f;
    public float suspensionDist = 0.3f;
    public float springForce = 25000f;
    public float damperForce = 4500f;
    public float vehicleMass = 1200f;
    public Vector3 centerOfMassOffset = new Vector3(0f, -0.5f, 0f);
}
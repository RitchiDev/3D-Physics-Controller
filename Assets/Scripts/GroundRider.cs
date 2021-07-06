using UnityEngine;

public class GroundRider : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private float m_Radius;
    [SerializeField] private float m_MaxRayDistance;
    [SerializeField] private LayerMask m_LayerMask;

    [Header("Ride Settings")]
    [SerializeField] private bool m_DoRide = true;
    [SerializeField] private float m_RideHeight = 1.1f;
    [SerializeField] private float m_RideSpringStrength = 10f;
    [SerializeField] private float m_RideSpringDampener = 10f;
    private Vector3 m_RayOffset;

    [Header("Upright Rotation Settings")]
    [SerializeField] private bool m_DoUprightRotation = true;
    [SerializeField] private float m_UprightJointSpringStrength = 10f;
    [SerializeField] private float m_UprightJointSpringDamper = 10f;

    private Rigidbody m_Rigidbody;
    private Collider m_Collider;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_RayOffset = new Vector3(0, m_Collider.bounds.size.y * 0.5f, 0);
    }

    private void FixedUpdate()
    {
        UpdateRideHeight();
        UpdateUprightForce();
    }

    private void UpdateRideHeight()
    {
        if (!m_DoRide)
        {
            return;
        }

        RaycastHit hit;
        bool rayDidHit = Physics.Raycast(m_Rigidbody.position, m_Rigidbody.transform.TransformDirection(Vector3.down), out hit, m_MaxRayDistance, m_LayerMask);
        Debug.DrawRay(m_Rigidbody.position, m_Rigidbody.transform.TransformDirection(Vector3.down) * m_MaxRayDistance, Color.red);

        if (rayDidHit)
        {
            Vector3 vel = m_Rigidbody.velocity;
            Vector3 rayDir = m_Rigidbody.transform.TransformDirection(Vector3.down);

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = hit.rigidbody;
            if (hitBody != null)
            {
                otherVel = hitBody.velocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = hit.distance - m_RideHeight;

            float springForce = (x - m_RideSpringStrength) - (relVel * m_RideSpringDampener);

            m_Rigidbody.AddForce(rayDir * springForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, hit.point);
            }
        }
    }

    private void UpdateUprightForce()
    {
        if (!m_DoUprightRotation)
        {
            return;
        }

        Quaternion upRightJointTargetRot = new Quaternion(0f, m_Rigidbody.rotation.y, 0f, m_Rigidbody.rotation.w);
        //Quaternion upRightJointTargetRot = new Quaternion(0f, m_Rigidbody.rotation.y, 0f, 0f);
        Quaternion characterCurrent = m_Rigidbody.transform.rotation;

        Quaternion toGoal = ShortestRoation(upRightJointTargetRot, characterCurrent);

        Vector3 rotAxis;
        float rotDegrees;

        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();

        float rotRadians = rotDegrees * Mathf.Deg2Rad;

        m_Rigidbody.AddTorque((rotAxis * (rotRadians * m_UprightJointSpringStrength)) - (m_Rigidbody.angularVelocity * m_UprightJointSpringDamper));
    }

    public Quaternion ShortestRoation(Quaternion to, Quaternion from)
    {
        if (Quaternion.Dot(to, from) < 0)
        {
            return to * Quaternion.Inverse(Multiply(from, -1f));
        }
        else
        {
            return to * Quaternion.Inverse(from);
        }
    }

    public Quaternion Multiply(Quaternion input, float scalar)
    {
        return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
    }
}

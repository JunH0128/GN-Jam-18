using UnityEngine;

//kick method
public class AntennaWiggle : MonoBehaviour
{
    public Transform antenna; 
    public float stiffness = 120f;
    public float damping = 4f; // higher = less wiggle
    public float degreesPerUnit = 4f; // degrees of bend per unit of velocity
    public float maxAngle = 45f;
    public float sidewaysKick = 4f; // how much it bends when moving sideway
    public float verticalKick = 4f;
    public float movingJiggle = 0.3f;

    Rigidbody2D rb;
    Vector2 lastVel; // last frames velocity
    float bend; // Current Tilt
    float bendVel; // Tilt Speed

    void Awake()
    {
        if (antenna == null) 
            antenna = transform.Find("Freddy/Antenna");

        rb = GetComponentInParent<Rigidbody2D>();
    }

    void OnEnable()
    {
        bend = 0;
        bendVel = 0;
        if (rb != null) lastVel = rb.linearVelocity;
    }

    public void Kick(float amount)
    {
        bendVel += amount;
    }

    void Update()
    {
        float dt = Time.deltaTime;
        if (antenna == null || dt == 0) return;

        if (rb != null)
        {
            Vector2 vel = rb.linearVelocity;
            Vector2 diff = transform.InverseTransformDirection(vel - lastVel); // the difference in velocity 
            lastVel = vel;

            /* diff.y *= sidewaysKick; */
            /* diff.x *= sidewaysKick;  */
            bendVel -= diff.x * sidewaysKick; // bend in the direction of sideways movement
            bendVel += Mathf.Abs(diff.y) * verticalKick * Mathf.Sign(transform.lossyScale.x); // bend in the direction of the player facing
            bendVel += Random.Range(-1f, 1f) * vel.magnitude * movingJiggle * dt * 60f; // jiggle booty more when moving even faster
        }

        
        bendVel += (-bend * stiffness - bendVel * damping) * dt; // spring effect
        bend += bendVel * dt; 

        float angle = Mathf.Clamp(-bend * degreesPerUnit, -maxAngle, maxAngle); //clamp angle
        antenna.localRotation = Quaternion.Euler(0, 0, angle); // apply angle

    }
}

using UnityEngine;

// spins script
public class SpinOut : MonoBehaviour
{
    public Transform body; 
    public float minSpin = 400f, maxSpin = 750f; // degrees per second

    Rigidbody2D rb;
    bool wasGrounded;
    float spin, lastTurn;
    RaycastHit2D[] hits = new RaycastHit2D[1];

    void Awake() => rb = GetComponentInParent<Rigidbody2D>();

    void Update()
    {
        // brute force grounded check for errthing
        var filter = new ContactFilter2D { useTriggers = false };
        bool grounded = rb.IsTouching(filter) || rb.Cast(-transform.up, filter, hits, 0.2f) > 0;
        Vector2 vel = transform.InverseTransformDirection(rb.linearVelocity);

        if (wasGrounded && !grounded && vel.y > 2f) 
        {
            float dir = Mathf.Abs(vel.x) > 0.5f ? -Mathf.Sign(vel.x) : (Random.value < 0.5f ? -1f : 1f); // facing right
            spin = dir * Mathf.Sign(transform.lossyScale.x) * Random.Range(minSpin, maxSpin); // facing left
        }
        if (grounded) spin = 0; // reset spin 
        wasGrounded = grounded; 

        // fixing spin in the air
        float turned = Mathf.DeltaAngle(lastTurn, rb.rotation);
        lastTurn = rb.rotation;

        if (spin != 0) body.Rotate(0, 0, spin * Time.deltaTime - turned * Mathf.Sign(transform.lossyScale.x)); // spin
        else body.localRotation = Quaternion.Lerp(body.localRotation, Quaternion.identity, 20f * Time.deltaTime); // lerp back to upright
        /*  else localRotation = Quaternion.Lerp(body.localRotation, Quaternion.identity, 20f * Time.deltaTime); */
    }
}

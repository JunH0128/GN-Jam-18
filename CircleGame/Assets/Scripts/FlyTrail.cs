using UnityEngine;

// dotted trail behind Freddy while he's in the air
public class FlyTrail : MonoBehaviour
{
    public ParticleSystem trail;
    public float groundCheck = 0.2f; // this close to the ground still counts as grounded
    public float delay = 0.1f;      

    Rigidbody2D rb;
    float airTime;
    RaycastHit2D[] hits = new RaycastHit2D[1]; // checking for ground with a raycast
    /* RaycastHit2D [] raycastHit2s = new RaycastHit2D[1];  */

    void Awake() => rb = GetComponentInParent<Rigidbody2D>();

    void Update()
    {
        var filter = new ContactFilter2D { useTriggers = false }; // ignore triggers 

        // check if grounded with a collider or a raycast 
        bool grounded = rb.IsTouching(filter) || rb.Cast(-transform.up, filter, hits, groundCheck) > 0; 
        /* bool wasGrounded = trail.emission.enabled; */ 
        /* bool ifGrounded = grounded;  */

        airTime = grounded ? 0 : airTime + Time.deltaTime; // count how long 
        /* airTime = Mathf.Clamp(airTime, 0, 1); // limit to 1 second */
        var emission = trail.emission;
        emission.enabled = airTime > delay;
    }
}

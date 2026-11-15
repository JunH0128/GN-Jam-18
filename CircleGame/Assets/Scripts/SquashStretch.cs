using UnityEngine;


public class SquashStretch : MonoBehaviour
{
    public Transform body;        
    public float feetY = -0.37f;  // feet position. *FIXglitch alignment 
    public float stiffness = 170f, damping = 9f; // test to change the spring of the squash/stretch
    public float jumpStretch = 6f, landSquash = 7f;

    Rigidbody2D rb;
    bool wasGrounded;
    float fallSpeed, airTime, squash, squashVel;
    RaycastHit2D[] hits = new RaycastHit2D[1];

    void Awake() => rb = GetComponentInParent<Rigidbody2D>();

    void Update()
    {
        var filter = new ContactFilter2D { useTriggers = false };
        bool grounded = rb.IsTouching(filter) || rb.Cast(-transform.up, filter, hits, 0.2f) > 0; //FIX tweaking out when spinning
        float upSpeed = transform.InverseTransformDirection(rb.linearVelocity).y;

        if (wasGrounded && !grounded && upSpeed > 2f) squashVel += jumpStretch;                    // jumped
        if (!wasGrounded && grounded && fallSpeed > 2f && airTime > 0.15f) squashVel -= landSquash; // landed after a real jump/fall

        wasGrounded = grounded;
        fallSpeed = grounded ? 0 : Mathf.Max(fallSpeed, -upSpeed); // fastest fall since leaving the ground
        airTime = grounded ? 0 : airTime + Time.deltaTime;


        ///Squash Methods///
        squashVel += (-squash * stiffness - squashVel * damping) * Time.deltaTime;
        squash += squashVel * Time.deltaTime;
        float s = Mathf.Clamp(squash, -0.5f, 0.6f); // limiter edit
        body.localScale = new Vector3(1 - s * 0.55f, 1 + s, 1); // localScale to squash/stretch the body. Edit.
        body.localPosition = new Vector3(0, -feetY * s, 0);     // keep feet on ground
    }
}

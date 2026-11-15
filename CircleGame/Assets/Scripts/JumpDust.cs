using UnityEngine;

public class JumpDust : MonoBehaviour
{
    public ParticleSystem puff;
    public int amount = 8;
    public BoingSound sound; // boing when jumping off a planet

    Rigidbody2D rb;
    bool wasGrounded, onPlanet;

    RaycastHit2D[] hits = new RaycastHit2D[1];

    void Awake() => rb = GetComponentInParent<Rigidbody2D>();

    void Update()
    {
        var filter = new ContactFilter2D { useTriggers = false };
        bool near = rb.Cast(-transform.up, filter, hits, 0.2f) > 0;
        bool grounded = rb.IsTouching(filter) || near;
        float upSpeed = transform.InverseTransformDirection(rb.linearVelocity).y; // get y velocity
        if (near) onPlanet = hits[0].transform.GetComponent("PlanetGrav") != null; // remember what we're standing on


        if (wasGrounded && !grounded && upSpeed > 2f) // if player was grounded and now in the air emit dust
        {
            puff.Emit(amount);
            if (onPlanet && sound != null) sound.Jump();
        }

        wasGrounded = grounded;
    }
}

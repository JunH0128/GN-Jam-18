using UnityEngine;

public class JumpDust : MonoBehaviour
{
    public ParticleSystem puff;
    public int amount = 8;

    Rigidbody2D rb;
    bool wasGrounded;

    void Awake() => rb = GetComponentInParent<Rigidbody2D>();

    void Update()
    {
        bool grounded = rb.IsTouching(new ContactFilter2D { useTriggers = false });
        float upSpeed = transform.InverseTransformDirection(rb.linearVelocity).y; // get y velocity

        
        if (wasGrounded && !grounded && upSpeed > 2f) // if player was grounded and now in the air emit dust
            puff.Emit(amount);

        wasGrounded = grounded;
    }
}

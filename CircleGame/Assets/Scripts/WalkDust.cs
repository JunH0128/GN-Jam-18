using UnityEngine;

public class WalkDust : MonoBehaviour
{
    public ParticleSystem dust;
    Rigidbody2D rb;

    void Awake() => rb = GetComponentInParent<Rigidbody2D>();

    void Update()
    {
        var emission = dust.emission; 

        /* emission.enabled = rb.IsTouchingLayers(LayerMask.GetMask("Ground")); */
        emission.enabled = rb.IsTouching(new ContactFilter2D { useTriggers = false }); // ignores triggers that don't count as ground
    }
}

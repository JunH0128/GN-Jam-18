using UnityEngine;

// burst of dust when landing after a jump or fall
public class LandDust : MonoBehaviour
{
    public ParticleSystem dust;
    public int amount = 14;
    public float minImpact = 2f; // ignore tiny bumps
    public float jelly = 0.1f;
    public ScreenShake shake;    // shakes the camera on planet landings
    public BoingSound sound;     // boing on planet landings

    Rigidbody2D rb;
    bool wasGrounded; //check
    float fallSpeed, airTime; // fastest fall and time in the air since leaving the ground
    RaycastHit2D[] hits = new RaycastHit2D[1];

    Transform planet;        
    Vector3 planetScale;     // normal size
    float wobbleTime = 1f;

    void Awake() => rb = GetComponentInParent<Rigidbody2D>();

    void Update()
    {
        
        var filter = new ContactFilter2D { useTriggers = false }; //FIX
        bool grounded = rb.IsTouching(filter) || rb.Cast(-transform.up, filter, hits, 0.2f) > 0;


        if (!wasGrounded && grounded && fallSpeed > minImpact && airTime > 0.15f) // just landed after a jump or fall
        {
            dust.Emit(amount);
            if (rb.Cast(-transform.up, filter, hits, 0.3f) > 0) Wobble(hits[0].transform); // squish what we landed on
        }

        //wobble
        if (planet != null && wobbleTime < 0.6f)
        {
            wobbleTime += Time.deltaTime;
            float w = wobbleTime < 0.6f ? jelly * Mathf.Sin(wobbleTime * 30f) * (1 - wobbleTime / 0.6f) : 0;
            planet.localScale = new Vector3(planetScale.x * (1 + w), planetScale.y * (1 - w), planetScale.z);
        }

        // remember fall speed here
        fallSpeed = grounded ? 0 : Mathf.Max(fallSpeed, -transform.InverseTransformDirection(rb.linearVelocity).y);
        airTime = grounded ? 0 : airTime + Time.deltaTime;
        wasGrounded = grounded;
    }

    void Wobble(Transform t)
    {
        if (t.GetComponent("PlanetGrav") == null) return;  // only planets
        if (shake != null) shake.Shake();
        if (sound != null) sound.Land();
        if (planet != null && planet != t) planet.localScale = planetScale; // FIX last planet
        if (planet != t) planetScale = t.localScale; // remember normal size
        planet = t;
        wobbleTime = 0;
    }
}

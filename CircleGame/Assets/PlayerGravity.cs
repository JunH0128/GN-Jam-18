
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerGravity : MonoBehaviour //PLAYER MOMENT IS USED HERE INSTEAD
{
    public float moveSpeed = 5f; //moved player movement to this script
    public float normalJumpForce = 7f;
    public float normalGravity = 1f;

    public float planetAcceleration = 12f; //these settings take over once the player enters a planets gravity
    public float maxPlanetSpeed = 12f;
    public float planetJumpSpeed = 12f;
    public float gravityMultiplier = 1f;

    public float adhesionStrength = 5f; //stops player from flying off easily, how strongly the player is kept against the surface

    public float planetLinearDamping = 0.05f; //this lets the player keep momentum, creates the slippery effect

    public float rotationSpeed = 12f; //for when moving around the planet

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private SpriteRenderer spriteRenderer;

    private readonly List<PlanetGrav> fieldsInRange = new List<PlanetGrav>(); //tracks every gravity field the player is inside

    private float moveInput;
    private bool normalGrounded;
    private bool planetGrounded;

    private PlanetGrav currentPlanet;
    private float originalDamping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalDamping = rb.linearDamping; //the original damping can be restored outside a planets gravity settings

        rb.gravityScale = normalGravity; //standard platforming physics
        rb.freezeRotation = true;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.W)) moveInput += 1f;
        if (Input.GetKey(KeyCode.S)) moveInput -= 1f;

        moveInput = Mathf.Clamp(moveInput, -1f, 1f);

        if (spriteRenderer != null) //flips the sprite for when we add one later
        {
            if (moveInput > 0)
                spriteRenderer.flipX = false;
            else if (moveInput < 0)
                spriteRenderer.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentPlanet != null && planetGrounded)
                PlanetJump();
            else if (currentPlanet == null && normalGrounded)
                NormalJump();
        }
    }

    private void FixedUpdate() //all physics handled here
    {
        currentPlanet = GetNearestField();

        if (currentPlanet == null)
        {
            rb.gravityScale = normalGravity;
            rb.linearDamping = originalDamping;
            rb.freezeRotation = true;

            rb.linearVelocity = new Vector2(
                moveInput * moveSpeed,
                rb.linearVelocity.y
            );

            planetGrounded = false;
            return;
        }

        rb.gravityScale = 0f; //normal gravity disabled as we handle our own gravity direction
        rb.linearDamping = planetLinearDamping;
        rb.freezeRotation = false;

        Vector2 center = currentPlanet.transform.position;
        Vector2 surfaceUp = (rb.position - center).normalized;
        Vector2 tangent = new Vector2(surfaceUp.y, -surfaceUp.x);

        float gravity = currentPlanet.gravityStrength * gravityMultiplier;

        rb.AddForce(-surfaceUp * gravity);

        if (currentPlanet.surfaceCollider != null) //checks distance between player and planets surface
        {
            ColliderDistance2D contact = Physics2D.Distance(
                playerCollider,
                currentPlanet.surfaceCollider
            );

            planetGrounded = contact.isOverlapped ||
                             contact.distance <= 0.08f;
        }
        else
        {
            planetGrounded = false;
        }

        if (planetGrounded)
        {
            float radialVelocity = Vector2.Dot( //calculates players velocity towards and away from planet
                rb.linearVelocity,
                surfaceUp
            );

            if (radialVelocity < 0f)
                radialVelocity = 0f;

            float tangentVelocity = Vector2.Dot(
                rb.linearVelocity,
                tangent
            );

            rb.linearVelocity = //seperate players movement into surface movement and radial movement
                tangent * tangentVelocity +
                surfaceUp * radialVelocity;


            rb.AddForce(
                tangent * moveInput * planetAcceleration,
                ForceMode2D.Force
            );

            float speed = Vector2.Dot(rb.linearVelocity, tangent); //limits player speed around the planet

            if (Mathf.Abs(speed) > maxPlanetSpeed)
            {
                rb.linearVelocity =
                    tangent * Mathf.Sign(speed) * maxPlanetSpeed +
                    surfaceUp * radialVelocity;
            }
        }
        else
        {

        }

        float targetAngle = //angle needed for player to face away from planet
            Mathf.Atan2(surfaceUp.y, surfaceUp.x) *
            Mathf.Rad2Deg - 90f;

        rb.MoveRotation(
            Mathf.LerpAngle(
                rb.rotation,
                targetAngle,
                rotationSpeed * Time.fixedDeltaTime
            )
        );

        if (planetGrounded) //apply extra force towards planet to prevent player flying off
        {
            float speed = rb.linearVelocity.magnitude;
            float requiredGravity = (speed * speed) /
                Mathf.Max(0.1f, Vector2.Distance(rb.position, center));

            rb.AddForce(
                -surfaceUp * Mathf.Max(0f, requiredGravity - gravity + adhesionStrength),
                ForceMode2D.Force
            );
        }
    }

    private void NormalJump() //used for ourside planet gravity settings
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            normalJumpForce
        );

        normalGrounded = false;
    }

    private void PlanetJump()
    {
        if (currentPlanet == null) return;

        Vector2 surfaceUp = //find direction pointing away from planets centre
            (rb.position - (Vector2)currentPlanet.transform.position).normalized;

        rb.linearVelocity += surfaceUp * planetJumpSpeed; //adds outward velocity to launch player

        planetGrounded = false;
    }

    private PlanetGrav GetNearestField() 
    {
        PlanetGrav nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (PlanetGrav planet in fieldsInRange)
        {
            if (planet == null) continue;

            float distance = Vector2.Distance(
                rb.position,
                planet.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = planet;
            }
        }

        return nearest;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            normalGrounded = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            normalGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            normalGrounded = false;
    }

    public void EnterField(PlanetGrav planet)
    {
        if (!fieldsInRange.Contains(planet))
            fieldsInRange.Add(planet);
    }

    public void ExitField(PlanetGrav planet)
    {
        fieldsInRange.Remove(planet);
    }
} //scientists calculated: hi

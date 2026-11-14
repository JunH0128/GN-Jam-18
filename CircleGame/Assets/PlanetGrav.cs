
using UnityEngine;

public class PlanetGrav : MonoBehaviour
{
    public float gravityStrength = 90f;

    public CircleCollider2D surfaceCollider;
    public CircleCollider2D gravityFieldCollider;

    private void Awake()
    {
        if (surfaceCollider == null)
            surfaceCollider = GetComponent<CircleCollider2D>();

        if (gravityFieldCollider != null)
            gravityFieldCollider.isTrigger = true;

        if (surfaceCollider != null)
            surfaceCollider.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D other) //called when player enters gravity field
    {
        if (gravityFieldCollider == null) return;
        if (!other.CompareTag("Player")) return;

        PlayerGravity pg = other.GetComponent<PlayerGravity>(); //gets the players gravity script

        if (pg != null)
            pg.EnterField(this);
    }

    private void OnTriggerExit2D(Collider2D other) //called when player leaves gravity field
    {
        if (gravityFieldCollider == null) return;
        if (!other.CompareTag("Player")) return;

        PlayerGravity pg = other.GetComponent<PlayerGravity>();

        if (pg != null)
            pg.ExitField(this);
    }
}

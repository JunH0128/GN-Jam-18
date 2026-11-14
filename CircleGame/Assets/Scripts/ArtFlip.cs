using UnityEngine;

public class ArtFlip : MonoBehaviour
{
    SpriteRenderer parentSprite; 

    void Awake() => parentSprite = transform.parent ? transform.parent.GetComponent<SpriteRenderer>() : null; // Get sprite renderer of parent object

    void LateUpdate()
    {
        if (parentSprite == null) return; 

        Vector3 s = transform.localScale; // set local scale
        s.x = Mathf.Abs(s.x) * (parentSprite.flipX ? -1 : 1); //Math the x scale of the sprite flipX 
        transform.localScale = s; // Use new scale 
    }
}

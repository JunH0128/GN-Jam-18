using UnityEngine;


public class MouthSwap : MonoBehaviour
{
    public GameObject mouth, mouthOpen;
    Rigidbody2D rb;
    float airTime; 

    void Awake() => rb = GetComponentInParent<Rigidbody2D>();

    void Update()
    {
        
        airTime = rb.IsTouching(new ContactFilter2D { useTriggers = false }) ? 0 : airTime + Time.deltaTime; // if player touches the ground reset airtime to 0
        bool open = airTime > 0.08f; // if player is in air for more than this number open mouth
        mouth.SetActive(!open); 
        mouthOpen.SetActive(open);  // swapper
    }
}

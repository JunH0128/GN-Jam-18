using UnityEngine;


[DefaultExecutionOrder(1000)]
public class ScreenShake : MonoBehaviour
{
    public float strength = 0.15f; 
    public float duration = 0.2f;  

    Transform cam;
    Vector3 offset;
    float timeLeft;

    void Awake() => cam = Camera.main.transform;

    public void Shake() => timeLeft = duration;

    void Update() => cam.position -= offset; 

    void LateUpdate()
    {
       
        offset = timeLeft > 0 ? (Vector3)Random.insideUnitCircle * strength * (timeLeft / duration) : Vector3.zero;
        timeLeft -= Time.deltaTime;
        cam.position += offset;
    }
}

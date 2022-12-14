using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEffect : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float rate;
    ParticleSystem speedFX;
    [SerializeField] Gradient effectColor;
    float walkSpeed;
    float maxSpeed;
    [SerializeField] TrailRenderer trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        speedFX = GetComponent<ParticleSystem>();
        walkSpeed = PlayerController.Instance.WalkSpeed();
        maxSpeed = PlayerController.Instance.SpeedMax();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float playerVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        speedFX.emissionRate =  (playerVelocity - walkSpeed) * rate;
        Color colorBySpeed = effectColor.Evaluate((playerVelocity - walkSpeed) / (maxSpeed - walkSpeed));
        speedFX.startColor = colorBySpeed;
        trailRenderer.startColor = colorBySpeed;
    }
}

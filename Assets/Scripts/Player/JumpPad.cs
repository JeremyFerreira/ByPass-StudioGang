using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float force;

    [SerializeField] LayerMask playerLayer;
    [SerializeField] AudioComponent audioJump;
    float timeToEnter;
    [SerializeField] ShakeData shake;

    private void Update()
    {
        timeToEnter-=Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<Rigidbody>() != null && other.gameObject.layer == 3 && timeToEnter<0)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            other.GetComponent<PlayerController>().SetCanDoubleJump(true);

            if (other.TryGetComponent<SlowTime>(out SlowTime slowTime) && slowTime.IsSlowTime())
            {
                rb.AddForce(force * transform.up * 1.5f, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(force * transform.up, ForceMode.Impulse);
            }


            audioJump.PlayAudioCue();
            CameraShakeManager.instance.Shake(shake);
            timeToEnter = 0.5f;
        }
    }
}

using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ventilateur : MonoBehaviour
{
    [SerializeField] AudioComponent audioVentilateurIdle;
    [SerializeField] AudioComponent audioVentilateurIn;
    public float force;
    [SerializeField] ShakeData shake;
    [SerializeField] RumblerDataConstant rumbleVentilo;

    private void Start()
    {
        audioVentilateurIdle.PlayAudioCue();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb) && other.gameObject.layer == 3)
        {

            Vector3 velocity = rb.velocity;

            //addVelocity
            velocity += transform.up * force * 1/(rb.position-transform.position).magnitude * Time.deltaTime;

            //apply velocity
            rb.velocity = velocity;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb) && other.gameObject.layer == 3)
        {
            audioVentilateurIn.PlayAudioCue();
            CameraShakeManager.instance.Shake(shake);
            Rumbler.instance.RumbleConstant(rumbleVentilo);
        }
        if(other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.SetCanDoubleJump(true);
        }
    }
}

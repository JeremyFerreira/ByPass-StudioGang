using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ventilateur : MonoBehaviour
{
    public float force;

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
}

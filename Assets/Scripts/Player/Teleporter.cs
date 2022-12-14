using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform destination;
    [SerializeField] AudioComponent audioTP;

    // Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<Rigidbody>() != null)
        {
            col.transform.position = destination.position;
            audioTP.PlayAudioCue();
        }
    }
}

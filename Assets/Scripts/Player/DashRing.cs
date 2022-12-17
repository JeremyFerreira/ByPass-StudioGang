using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRing : MonoBehaviour
{
    [SerializeField] AudioComponent audioDash;
    [SerializeField] RumblerDataConstant dashRumble;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            controller.Dash();
            audioDash.PlayAudioCue();
            Rumbler.instance.RumbleConstant(dashRumble);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private EventSO _deathEvent;
    [SerializeField] private LayerMask _layersDeath;

    private void OnTriggerEnter(Collider other)
    {
        if ((_layersDeath & (1 << other.gameObject.layer)) != 0)
        {
             _deathEvent.OnLaunchEvent.Invoke();
        }
    }


}

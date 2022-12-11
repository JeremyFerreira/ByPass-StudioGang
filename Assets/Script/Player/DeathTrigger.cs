using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private EventSO _deathEvent;
    [SerializeField] private EventSO _StartRunEvent;
    [SerializeField] private LayerMask _layersDeath;

    bool _IsAlive;
    private void OnEnable()
    {
        _StartRunEvent.OnLaunchEvent += Initialized;
    }
    private void OnDisable()
    {
        _StartRunEvent.OnLaunchEvent -= Initialized;
    }
    private void Initialized()
    {
        _IsAlive = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((_layersDeath & (1 << other.gameObject.layer)) != 0)
        {
            if (_IsAlive)
            {
                _IsAlive = false;
                _deathEvent.OnLaunchEvent.Invoke();
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private EventSO _deathEvent;
    [SerializeField] private EventSO _winEvent;
    [SerializeField] private EventSO _StartRunEvent;
    [SerializeField] private EventSO _eventStopInput;
    [SerializeField] private LayerMask _layersDeath;

    bool _IsAlive;
    bool _IsWin;
    private void OnEnable()
    {
        _StartRunEvent.OnLaunchEvent += Initialized;
        _winEvent.OnLaunchEvent += Win;
        _IsWin = false;
    }
    private void OnDisable()
    {
        _StartRunEvent.OnLaunchEvent -= Initialized;
        _winEvent.OnLaunchEvent -= Win;
    }
    private void Win()
    {
        _IsWin = true;
    }
    private void Initialized()
    {
        _IsAlive = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((_layersDeath & (1 << other.gameObject.layer)) != 0)
        {
            if (_IsAlive && !_IsWin)
            {
                _IsAlive = false;
                _deathEvent.OnLaunchEvent?.Invoke();
                _eventStopInput.OnLaunchEvent?.Invoke();
            }
        }
    }


}

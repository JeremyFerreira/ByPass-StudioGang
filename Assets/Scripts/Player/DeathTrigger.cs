using FirstGearGames.SmoothCameraShaker;
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

    [SerializeField] AudioComponent _audioComponent;
    [SerializeField] ShakeData deathShakeData;

    public bool _IsAlive;
    bool _IsWin;
    [SerializeField] RestartLevel restart;
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
                if(!restart.hasRestarted)
                {
                    _deathEvent.OnLaunchEvent?.Invoke();
                    _eventStopInput.OnLaunchEvent?.Invoke();
                }
               
                _audioComponent.PlayAudioCue();
                CameraShakeManager.instance.Shake(deathShakeData);
            }
        }
    }


}

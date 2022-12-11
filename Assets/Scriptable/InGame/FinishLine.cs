using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] EventSO _reachFinishLine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Layer Player
            _reachFinishLine.OnLaunchEvent?.Invoke();
    }

    private void OnEnable()
    {
        _reachFinishLine.OnLaunchEvent += Debuglog;
    }

    private void Debuglog()
    {
        Debug.Log("hey");
    }
}

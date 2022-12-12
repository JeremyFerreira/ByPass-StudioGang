using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] EventSO _reachFinishLine;

    bool _alreadyTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Layer Player
            if (!_alreadyTrigger)
            {
                _alreadyTrigger = true;
                _reachFinishLine.OnLaunchEvent?.Invoke();
            }
    }   
}

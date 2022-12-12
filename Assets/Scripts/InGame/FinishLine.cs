using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] EventSO _reachFinishLine;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // Layer Player
            _reachFinishLine.OnLaunchEvent?.Invoke();
    }
}

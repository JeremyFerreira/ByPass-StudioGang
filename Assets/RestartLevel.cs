using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartLevel : MonoBehaviour
{
    [SerializeField] SOInputButton sOInputButton;
    [SerializeField] EventSO restartEvent;
    // Start is called before the first frame update
    void OnEnable()
    {
        sOInputButton.OnPressed += RestartTheLevel;
    }
    void OnDisable()
    {
        sOInputButton.OnPressed -= RestartTheLevel;
    }
    public void RestartTheLevel()
    {
        restartEvent.OnLaunchEvent.Invoke();
    }

}

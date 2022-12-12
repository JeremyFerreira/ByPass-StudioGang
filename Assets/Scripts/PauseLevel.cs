using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseLevel : MonoBehaviour
{
    [SerializeField] SOInputButton sOInputButton;
    [SerializeField] EventSO pauseEvent;
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
        pauseEvent.OnLaunchEvent.Invoke();
    }
}

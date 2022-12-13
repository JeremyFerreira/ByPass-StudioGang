using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseLevel : MonoBehaviour
{
    [SerializeField] SOInputButton sOInputButton;
    [SerializeField] EventSO pauseEvent;

    bool canClick = true;
    // Start is called before the first frame update
    void OnEnable()
    {
        sOInputButton.OnPressed += Pause;

    }
    void OnDisable()
    {
        sOInputButton.OnPressed -= Pause;
    }
    public void Pause()
    {
        if(canClick)
        {
            pauseEvent.OnLaunchEvent?.Invoke();
            StartCoroutine(DoCheck());
        }
    }

    IEnumerator DoCheck()
    {
        canClick = false;
        yield return new WaitForSecondsRealtime(.8f);
        canClick = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testbutton : MonoBehaviour
{
    public EventSO myevent;
public void click()
    {
        myevent.OnLauchEventInt?.Invoke(2);
    }
}

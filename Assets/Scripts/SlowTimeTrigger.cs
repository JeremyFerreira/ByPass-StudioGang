using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeTrigger : MonoBehaviour
{
    [SerializeField] EventSO startSlowTime;
    [SerializeField] EventSO endSlowTime;
    [SerializeField] Collider objectCollider;
    private void OnEnable()
    {
        startSlowTime.OnLaunchEvent += ActiveTrigger;
        endSlowTime.OnLaunchEvent += DisactiveTrigger;
    }
    private void OnDisable()
    {
        startSlowTime.OnLaunchEvent -= ActiveTrigger;
        endSlowTime.OnLaunchEvent -= DisactiveTrigger;
    }
    // Start is called before the first frame update
    void Start()
    {
        //DisactiveTrigger();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ActiveTrigger()
    {
        //objectCollider.enabled = true;
    }
    private void DisactiveTrigger()
    {
        //objectCollider.enabled = false;
    }
}

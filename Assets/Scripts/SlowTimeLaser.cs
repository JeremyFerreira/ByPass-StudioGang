using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeLaser : MonoBehaviour
{
    [SerializeField] EventSO startSlowTime;
    [SerializeField] EventSO endSlowTime;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Collider objectCollider;
    float timer = 0.15f;

    bool launchTimer;

    private void OnEnable()
    {
        startSlowTime.OnLaunchEvent += DisactiveRenderer;
        endSlowTime.OnLaunchEvent += LaunchTimer;
        endSlowTime.OnLaunchEvent += ActiveRenderer;
    }
    private void OnDisable()
    {
        startSlowTime.OnLaunchEvent -= DisactiveRenderer;
        endSlowTime.OnLaunchEvent -= LaunchTimer;
        endSlowTime.OnLaunchEvent -= ActiveRenderer;
    }
    // Start is called before the first frame update
    void Start()
    {
        ActiveRenderer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(launchTimer)
        {
            timer -= Time.fixedDeltaTime;
            if(timer < 0)
            {
                ActiveCollider();
                timer = 0.15f;
                launchTimer = false;
            }
        }
    }
    private void LaunchTimer()
    {
        launchTimer = true;
    }
    private void ActiveRenderer()
    {
        meshRenderer.enabled = true;
    }
    private void ActiveCollider()
    {
        objectCollider.enabled = true;
    }
    private void DisactiveRenderer()
    {
        meshRenderer.enabled = false;
        objectCollider.enabled = false;
    }
}

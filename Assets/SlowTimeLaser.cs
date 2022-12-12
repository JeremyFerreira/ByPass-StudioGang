using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeLaser : MonoBehaviour
{
    [SerializeField] EventSO startSlowTime;
    [SerializeField] EventSO endSlowTime;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Collider objectCollider;

    private void OnEnable()
    {
        startSlowTime.OnLaunchEvent += DisactiveRenderer;
        endSlowTime.OnLaunchEvent += ActiveRenderer;
    }
    private void OnDisable()
    {
        startSlowTime.OnLaunchEvent -= DisactiveRenderer;
        endSlowTime.OnLaunchEvent -= ActiveRenderer;
    }
    // Start is called before the first frame update
    void Start()
    {
        ActiveRenderer();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ActiveRenderer()
    {
        meshRenderer.enabled = true;
        objectCollider.enabled = true;
    }
    private void DisactiveRenderer()
    {
        meshRenderer.enabled = false;
        objectCollider.enabled = false;
    }
}

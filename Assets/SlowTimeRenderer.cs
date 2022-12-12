using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeRenderer : MonoBehaviour
{
    [SerializeField] EventSO startSlowTime;
    [SerializeField] EventSO endSlowTime;
    [SerializeField] MeshRenderer meshRenderer;
    private void OnEnable()
    {
        startSlowTime.OnLaunchEvent += ActiveRenderer;
        endSlowTime.OnLaunchEvent += DisactiveRenderer;
    }
    private void OnDisable()
    {
        startSlowTime.OnLaunchEvent -= ActiveRenderer;
        endSlowTime.OnLaunchEvent -= DisactiveRenderer;
    }
    // Start is called before the first frame update
    void Start()
    {
        DisactiveRenderer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ActiveRenderer()
    {
        meshRenderer.enabled = true;
    }
    private void DisactiveRenderer()
    {
        meshRenderer.enabled = false;
    }
}

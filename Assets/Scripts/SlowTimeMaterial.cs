using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTimeMaterial : MonoBehaviour
{
    [SerializeField] EventSO startSlowTime;
    [SerializeField] EventSO endSlowTime;
    [SerializeField] Material matSlowTime;
    [SerializeField] Material matRealTime;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Collider col;
    public bool slowTime;
    public bool isUsing;

    private void OnEnable()
    {
        startSlowTime.OnLaunchEvent += MatSlowTime;
        startSlowTime.OnLaunchEvent += IsSlowTime;
        endSlowTime.OnLaunchEvent += MatRealime;
        endSlowTime.OnLaunchEvent += IsNotSlowTime;
    }
    private void OnDisable()
    {
        startSlowTime.OnLaunchEvent -= MatSlowTime;
        startSlowTime.OnLaunchEvent -= IsSlowTime;
        endSlowTime.OnLaunchEvent -= MatRealime;
        endSlowTime.OnLaunchEvent -= IsNotSlowTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        MatRealime();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void MatSlowTime()
    {
        meshRenderer.material = matSlowTime;
        col.enabled = true;
    }
    public void MatRealime()
    {
        if(!isUsing)
        {
            meshRenderer.material = matRealTime;
            col.enabled = false;
        }
    }
    void IsSlowTime()
    {
        slowTime = true;
    }
    void IsNotSlowTime()
    {
        slowTime = false;
    }
}

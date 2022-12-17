using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] Material watermat;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //time+= Time.deltaTime;
        //watermat.SetTextureOffset(1,new Vector2(time,0));
    }
}

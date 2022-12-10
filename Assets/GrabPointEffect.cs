using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabPointEffect : MonoBehaviour
{
    [SerializeField] GrapplingGun grapplingGun;
    [SerializeField] GameObject particule;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        particule.SetActive(grapplingGun.isGrappling);
        if(grapplingGun.GrapplePoint()!=null)
        transform.position = grapplingGun.GrapplePoint();
    }
}

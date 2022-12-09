using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] GrapplingGun grapplingGun;
    private Vector3 currentGrapplePosition;
    [SerializeField] int quality;
    private Spring spring;
    [SerializeField] float damper, strength, velocity, waveCount, waveHeight;
    [SerializeField] AnimationCurve affectCurve;
    // Start is called before the first frame update
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }
    void FixedUpdate()
    {
        DrawRope();
    }

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!grapplingGun.IsGrappling())
        {
            currentGrapplePosition = grapplingGun.gunTip.position;
            spring.Reset();
            if (lr.positionCount > 0)
            {
                lr.positionCount = 0;
            }
            return;
        }

        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        var grapplePoint = grapplingGun.GetGrapplePoint();
        var gunTipPosition = grapplingGun.gunTip.position;
        var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        currentGrapplePosition = grapplePoint;
        for (int i = 0; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI * spring.Value * affectCurve.Evaluate(delta));
            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
    }
}


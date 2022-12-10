using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [SerializeField] float forcePull;
    //The distance grapple will try to keep from grapple point. 
    [SerializeField] float maxDistance;
    [SerializeField] float minDistance;

    [SerializeField] float maxDistanceShoot;

    [SerializeField] float damper;
    [SerializeField] float jointspring;
    [SerializeField] float massScale;

    [SerializeField] int positionCount;
    
    private Vector3 grapplePoint;
    public Vector3 GrapplePoint()
        { return grapplePoint; }

    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private SpringJoint joint;
    public bool isGrappling;
    public PlayerController playerController;
    public bool canGrapple;
    [SerializeField] SlowTime slowTime;
    RaycastHit hit;
    [SerializeField] SOInputButton grapplingInput;

    [SerializeField] EventSO startRun;
    [SerializeField] EventSO stopRun;

    private void EnableInput()
    {
        grapplingInput.OnPressed += StartGrapple;
        grapplingInput.OnReleased += StopGrapple;
    }
    private void DisableInput()
    {
        grapplingInput.OnPressed -= StartGrapple;
        grapplingInput.OnReleased -= StopGrapple;
    }
    private void Start()
    {
        startRun.OnLaunchEvent += EnableInput;
        stopRun.OnLaunchEvent += DisableInput;
    }
    void Update()
    {
        playerController.SetGrappin(isGrappling);
        
        canGrapple = Physics.Raycast(camera.position, camera.forward, out hit, slowTime.IsSlowTime()? maxDistanceShoot * 1.5f : maxDistanceShoot, whatIsGrappleable);
            
    }
    private void FixedUpdate()
    {
        if (isGrappling)
        {
            playerController.GetComponent<Rigidbody>().AddForce((grapplePoint - transform.position) * forcePull);
        }
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    public void StartGrapple()
    {
        if (canGrapple)
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * minDistance;                ;
            joint.minDistance = distanceFromPoint * maxDistance;

            //Adjust these values to fit your game.
            joint.spring = jointspring;
            joint.damper = damper;
            joint.massScale = massScale;

            currentGrapplePosition = gunTip.position;

            isGrappling = true;
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    public void StopGrapple()
    {
        Destroy(joint);
        if (isGrappling)
        { 
            isGrappling = false;
            playerController.SetCanDoubleJump(true);
        }

    }

    private Vector3 currentGrapplePosition;

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
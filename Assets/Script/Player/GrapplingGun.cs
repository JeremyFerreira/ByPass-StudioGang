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
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private SpringJoint joint;
    Input input;
    public bool isGrappling;
    public PlayerController playerController;
    public bool canGrapple;
    
    void Awake()
    {
        input = new Input();
    }

   


    // LOOPS AND FUNCTIONS///////////////////////////////////////////////////////////////////

    private void OnEnable()
    {
        input.Enable();

        input.InGame.Grappling.performed += context => StartGrapple();
        input.InGame.Grappling.canceled += context => StopGrapple();
    }
    private void OnDisable()
    {
        input.Disable();

        input.InGame.Grappling.performed += context => StartGrapple();
        input.InGame.Grappling.canceled += context => StopGrapple();
    }
    void Update()
    {
        playerController.SetGrappin(isGrappling);
        RaycastHit hit;
        canGrapple = Physics.Raycast(camera.position, camera.forward, out hit, maxDistanceShoot, whatIsGrappleable);
            
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
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistanceShoot, whatIsGrappleable))
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
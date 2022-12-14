using FirstGearGames.SmoothCameraShaker;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    public static GrapplingGun instance;
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
    public LayerMask laserLayer;
    public Transform gunTip, cam, player;
    private SpringJoint joint;
    public bool isGrappling;
    public PlayerController playerController;
    public bool canGrapple;
    [SerializeField] SlowTime slowTime;
    RaycastHit hit;
    [SerializeField] SOInputButton grapplingInput;

    [SerializeField] EventSO startRun;
    [SerializeField] EventSO stopRun;

    [SerializeField] AudioComponent audioGrappinStart;
    [SerializeField] AudioComponent audioGrappinStop;
    [SerializeField] AudioComponent audioGrappinFail;
    [SerializeField] AudioComponent audioGrappinLock;
    [SerializeField] ShakeData shakeStart;
    [SerializeField] ShakeData shakeStop;
    [SerializeField] Animator grappinAnim;
    [SerializeField] Animator crossHair;
    [SerializeField] PlayerCam playerCam;
    bool hasViser;

    public GameObject grappinObject;
    [SerializeField] InputSO _inputSO;
    // LOOPS AND FUNCTIONS///////////////////////////////////////////////////////////////////
    private void OnEnable()
    {
        _inputSO.OnGrapplePressed += StartGrapple;
        _inputSO.OnGrappleReleased += StopGrapple;
    }
    private void OnDisable()
    {
        _inputSO.OnGrapplePressed -= StartGrapple;
        _inputSO.OnGrappleReleased -= StopGrapple;
    }
    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        playerController.SetGrappin(isGrappling);
        
        canGrapple = Physics.Raycast(cam.position, cam.forward, out hit, slowTime.IsSlowTime()? maxDistanceShoot * 1.5f : 0, whatIsGrappleable);
        if(canGrapple&& !isGrappling && !hasViser)
        {
            crossHair.CrossFade("CrossHairViser", 0, 0);
            hasViser = true;
            audioGrappinLock.PlayAudioCue();
        }
        if (!canGrapple && !isGrappling && hasViser)
        {
            crossHair.CrossFade("CrossHairViserInverse", 0, 0);
            hasViser = false;
            audioGrappinLock.PlayAudioCue();
        }
    }
    private void FixedUpdate()
    {
        if (isGrappling)
        {
            playerController.GetComponent<Rigidbody>().AddForce((grapplePoint - transform.position) * forcePull);
            if(Physics.Raycast(cam.position, grapplePoint-gunTip.position, out hit, (grapplePoint - gunTip.position).magnitude, laserLayer))
            {
                StopGrapple();
            }
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
            audioGrappinStart.PlayAudioCue();
            CameraShakeManager.instance.Shake(shakeStart);
            grappinAnim.CrossFade("GrappleStart", 0, 0);
            grappinObject = hit.collider.gameObject;
            grappinObject.GetComponent<SlowTimeMaterial>().isUsing = true;

            crossHair.CrossFade("CrossHairLock",0,0);
            playerCam.DoFov(playerCam.fovallRun);

        }
        else
        {
            audioGrappinFail.PlayAudioCue();
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
            audioGrappinStop.PlayAudioCue();
            CameraShakeManager.instance.Shake(shakeStop);
            grappinAnim.CrossFade("GrappleStop", 0, 0);

            SlowTimeMaterial slowTimeMaterial = grappinObject.GetComponent<SlowTimeMaterial>();
            slowTimeMaterial.isUsing = false;
            if (!slowTimeMaterial.slowTime && WallRunController.Instance.wall != grappinObject)
            {
                slowTimeMaterial.MatRealime();
            }
            grappinObject = null;
            crossHair.CrossFade("CrossHairUnlock", 0, 0);
            if(!playerController.wallrunning)
            {
                playerCam.DoFov(playerCam.GetFov());
            }
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
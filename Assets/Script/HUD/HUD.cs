using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject doubleJumpImage;
    public Image crossHair;
    public GrapplingGun grapplingGun;
    // Start is called before the first frame update
    void Start()
    {
        doubleJumpImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        crossHair.color = grapplingGun.canGrapple ? Color.red : Color.white;
        if(playerController.CanDoubleJump() && !playerController.IsGrounded())
        {
            doubleJumpImage.SetActive(true);
        }
        else
        {
            doubleJumpImage.SetActive(false);
        }
    }
}

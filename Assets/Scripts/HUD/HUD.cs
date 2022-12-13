using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject doubleJumpImage;
    // Start is called before the first frame update
    void Start()
    {
        doubleJumpImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerController.CanDoubleJump() && !playerController.IsGrounded())
        {
            doubleJumpImage.SetActive(true);
        }
        else
        {
            doubleJumpImage.SetActive(false);
        }
    }
}

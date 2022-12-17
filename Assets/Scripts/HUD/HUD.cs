using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject doubleJumpImage;
    [SerializeField] TextMeshProUGUI fps;
    float timerToChange;
    // Start is called before the first frame update
    void Start()
    {
        doubleJumpImage.SetActive(false);
        timerToChange = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerController.CanDoubleJump() || playerController.IsGrounded() ||playerController.wallrunning)
        {
            doubleJumpImage.SetActive(true);
        }
        else
        {
            doubleJumpImage.SetActive(false);
        }
        timerToChange -= Time.deltaTime;
        if(timerToChange < 0)
        {
            if (Settings.ShowFps)
            {
                float fpsCount = 1.0f / Time.unscaledDeltaTime;
                fps.text = Mathf.Ceil(fpsCount).ToString() + "fps"; ;
            }
            else
            {
                fps.text = "";
            }
            timerToChange = 0.1f;
        }
        
    }
}

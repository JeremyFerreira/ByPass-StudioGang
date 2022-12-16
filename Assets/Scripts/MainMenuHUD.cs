using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHUD : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
}

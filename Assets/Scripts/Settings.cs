using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Settings : MonoBehaviour
{
    public static Settings Instance;
    public void Awake()
    {
        Instance = this;
        StartCoroutine(DoCheck());
    }

    IEnumerator DoCheck()
    {

        yield return new WaitForSeconds(.2f);
        LoadAudioSettings();
        LoadKeyboardSettings();
        LoadGamePadSettings();
        LoadGraphicsSettings();
    }

    #region AudioSettings

    [Header("AUDIO DEFAULT SETTING")]
    [SerializeField]
    [Range(0, 0.8f)] float DefaultVolumeGeneral;
    [SerializeField]
    [Range(0, 0.8f)] float DefaultVolumeMusic;
    [SerializeField]
    [Range(0, 0.8f)] float DefaultVolumeSFX;

    public static float VolumeGeneral { get; set; }
    public static float VolumeMusic { get; set; }
    public static float VolumeSFX { get; set; }


    //Load Audio Settings on PlayerPref
    void LoadAudioSettings()
    {
        VolumeGeneral = PlayerPrefs.GetFloat("MasterVolume", DefaultVolumeGeneral);
        VolumeMusic = PlayerPrefs.GetFloat("MusicVolume", DefaultVolumeMusic);
        VolumeSFX = PlayerPrefs.GetFloat("SFXVolume", DefaultVolumeSFX);

        ModifyAudioManager();
    }

    //
    void ModifyAudioManager()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetGroupVolume("MasterVolume", VolumeGeneral);
            AudioManager.Instance.SetGroupVolume("MusicVolume", VolumeMusic);
            AudioManager.Instance.SetGroupVolume("SFXVolume", VolumeSFX);
        }
    }

    public void ChangeVolumeGeneral(float value)
    {
        VolumeGeneral = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        ModifyAudioManager();
    }

    public void ChangeVolumeMusic(float value)
    {
        VolumeMusic = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        ModifyAudioManager();
    }

    public void ChangeVolumeSFX(float value)
    {
        VolumeSFX = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        ModifyAudioManager();
    }
    #endregion

    #region KeyboardSettings
    [Header("KEYBOARD SETTING")]
    [SerializeField]
    [Range(0, 200f)] float DefaultSensibilityMouse;
    public static float SensibilityMouse { get; set; }
    void LoadKeyboardSettings()
    {
        SensibilityMouse = PlayerPrefs.GetFloat("SensibilityMouse", DefaultSensibilityMouse);
    }
    public static void ChangeValueSensibilityMouse(float value)
    {
        SensibilityMouse = value;
        PlayerPrefs.SetFloat("SensibilityMouse", value);
    }

    #endregion

    #region GamepadSettings
    [Header("GAMEPAD SETTING")]
    [SerializeField]
    [Range(100, 350f)] float _defaultSensibilityGamePad;
    [SerializeField] int _defaultUseRumbler;
    public static float SensibilityGamePad { get; set; }
    public static bool UseRumbler { get; set; }
    void LoadGamePadSettings()
    {
        SensibilityGamePad = PlayerPrefs.GetFloat("SensibilityGamePad", _defaultSensibilityGamePad);
        UseRumbler = PlayerPrefs.GetInt("UseRumbler", _defaultUseRumbler) == 1;
    }
    public static void ChangeValueSensibilityGamePad(float value)
    {
        SensibilityGamePad = value;
        PlayerPrefs.SetFloat("SensibilityGamePad", value);
    }

    public static void ChangeUseRumbler(bool value)
    {
        UseRumbler = value;
        PlayerPrefs.SetInt("UseRumbler", value ? 1 : 0);
        print(UseRumbler);
    }

    #endregion

    #region Graphics Settings
    [Header("GRAPHICS SETTING")]
    [SerializeField] int _defaultUseCameraShake;
    [SerializeField] int _defaultUseCameraClamp;
    [SerializeField] int _defaultShowFps;
    [SerializeField] int _defaultUseVsync;
    [SerializeField] int _defaultFpsTarget;
    [SerializeField] int _defaultQuality;
    [SerializeField] int _defaultAntiAliasing;
    [SerializeField] int _defaultScreenMode;

    public static bool UseCameraShake { get; set; }
    public static bool UseCameraClamp { get; set; }
    public static bool ShowFps { get; set; }
    public static bool UseVsync { get; set; }
    public static int FpsTarget { get; set; }
    public static int Quality { get; set; }
    public static int AntiAliasing { get; set; }
    public static int ScreenMode { get; set; }
    public static int ResolutionLevel { get; set; }

    public static Resolution[] StoreResolution;
    public static FullScreenMode ScreenModeType { get; set; }
    void LoadGraphicsSettings()
    {
        ChangeUseCameraShake(PlayerPrefs.GetInt("UseCameraShake", _defaultUseCameraShake) == 1);
        ChangeUseCameraClamp(PlayerPrefs.GetInt("UseCameraClamp", _defaultUseCameraClamp) == 1);
        ChangeShowFps(PlayerPrefs.GetInt("ShowFps", _defaultShowFps) == 1);
        ChangeVSync(PlayerPrefs.GetInt("UseVsync", _defaultUseVsync) == 1);
        ChangeFpsTarget(PlayerPrefs.GetInt("FpsTarget", _defaultFpsTarget));
        ChangeQuality(PlayerPrefs.GetInt("Quality", _defaultQuality));
        ChangeAntiAliasing(PlayerPrefs.GetInt("AntiAliasing", _defaultAntiAliasing));
        ChangeScreenMode(PlayerPrefs.GetInt("ScreenMode", _defaultScreenMode));
        InitializeResolution();

    }

    public static void ChangeScreenMode(int value)
    {
        ScreenMode = value;
        switch (ScreenMode)
        {
            case 0:
                ScreenModeType = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                ScreenModeType = FullScreenMode.Windowed;
                break;
            case 2:
                ScreenModeType = FullScreenMode.FullScreenWindow;
                break;
        }

        Screen.fullScreenMode = ScreenModeType;
        PlayerPrefs.SetInt("ScreenMode", ScreenMode);
    }

    public static void ChangeQuality(int quality)
    {
        Quality = quality;
        QualitySettings.SetQualityLevel(Quality);
        PlayerPrefs.SetInt("Quality", Quality);
    }

    public static void ChangeUseCameraShake(bool value)
    {
        UseCameraShake = value;
        PlayerPrefs.SetInt("UseCameraShake", value ? 1 : 0);
    }

    public static void ChangeUseCameraClamp(bool value)
    {
        UseCameraClamp = value;
        PlayerPrefs.SetInt("UseCameraClamp", value ? 1 : 0);
    }
    public static void ChangeShowFps(bool value)
    {
        ShowFps = value;
        PlayerPrefs.SetInt("ShowFps", value ? 1 : 0);

        /*if (HudControllerInGame.Instance != null)
        {
            if (ShowFps == false)
            {
                HudControllerInGame.Instance.SetShowFps(false);
            }
            else
            {
                HudControllerInGame.Instance.SetShowFps(true);
            }
        }*/
    }

    public static void ChangeVSync(bool value)
    {
        UseVsync = value;
        PlayerPrefs.SetInt("UseVsync", value ? 1 : 0);

        if (UseVsync == false)
        {
            QualitySettings.vSyncCount = 0;
        }
        else
        {
            QualitySettings.vSyncCount = 1;
        }
    }

    public static void ChangeFpsTarget(int value)
    {
        FpsTarget = value;
        switch (value)
        {
            case 0:
                Application.targetFrameRate = 60;
                break;
            case 1:
                Application.targetFrameRate = 120;
                break;
            case 2:
                Application.targetFrameRate = 150;
                break;
            case 3:
                Application.targetFrameRate = 300;
                break;
        }
        PlayerPrefs.SetInt("FpsTarget", value);
    }

    public static void ChangeAntiAliasing(int value)
    {
        AntiAliasing = value;
        QualitySettings.antiAliasing = (int)MathF.Pow(2f, AntiAliasing);
        PlayerPrefs.SetInt("AntiAliasing", AntiAliasing);
    }
    private void InitializeResolution()
    {
        Resolution[] resolution = Screen.resolutions;
        Array.Reverse(resolution);
        StoreResolution = resolution;
    }

    public static void ChangeResolution(int value)
    {
        ResolutionLevel = value;
        Screen.SetResolution(StoreResolution[value].width, StoreResolution[value].height, ScreenModeType);
        PlayerPrefs.SetInt("Resolution", ResolutionLevel);
    }

    #endregion
}

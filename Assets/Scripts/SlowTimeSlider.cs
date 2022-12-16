using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowTimeSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] ValueSo slowTimeSliderData;
    private void OnEnable()
    {
        slowTimeSliderData.OnBoolValueChange += slider.gameObject.SetActive;
        slowTimeSliderData.OnValueChange += ChangeSliderValue;
    }
    private void OnDisable()
    {
        slowTimeSliderData.OnBoolValueChange -= slider.gameObject.SetActive;
        slowTimeSliderData.OnValueChange -= ChangeSliderValue;
    }
    void ChangeSliderValue(float value)
    {
        slider.value = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

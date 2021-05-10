using UnityEngine;
using UnityEngine.UI;
using System;

public class UserInput : MonoBehaviour
{
    [Header("Objects with scripts")]
    [SerializeField] private GameObject pathSettings;
    [SerializeField] private GameObject objectSettings;
    [Header("Slider")]
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Text speedText;
    [Header("Text fields")]
    [SerializeField] private InputField minRandomInput;
    [SerializeField] private InputField maxRandomInput;


    // Start is called before the first frame update
    void Start()
    {
        speedSlider.maxValue = 50;
        speedSlider.minValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        SliderSet();
        speedText.text = $"Скорость движения {speedSlider.value}";
        PathGenSettings();
    }

    private void SliderSet()
    {
        objectSettings.GetComponent<FollowPath>().speed = speedSlider.value;
    }

    private void PathGenSettings()
    {
        pathSettings.GetComponent<PathGeneration>().minRandomRange = Convert.ToInt32(minRandomInput.text);
        pathSettings.GetComponent<PathGeneration>().maxRandomRange = Convert.ToInt32(maxRandomInput.text);
    }
}

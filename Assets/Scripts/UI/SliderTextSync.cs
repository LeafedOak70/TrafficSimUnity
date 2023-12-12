using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderTextSync : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField inputField;

    private void Start()
    {
        // Add listeners to handle changes in both slider and input field
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // Update the input field text with the slider value
        inputField.text = value.ToString();
    }

    private void OnInputFieldValueChanged(string value)
    {
        // Try to parse the input field text to a float
        if (float.TryParse(value, out float floatValue))
        {
            // Clamp the value within the slider's range
            floatValue = Mathf.Clamp(floatValue, slider.minValue, slider.maxValue);

            // Update the slider value with the input field value
            slider.value = floatValue;

            // Update the input field text with the clamped value
            inputField.text = floatValue.ToString();
        }
    }
}

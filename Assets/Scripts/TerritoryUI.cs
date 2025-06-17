using UnityEngine;
using UnityEngine.UI;

public class TerritoryUI : MonoBehaviour
{
    public Text redText;
    public Text blueText;
    public Slider redSlider;
    public Slider blueSlider;

    public void UpdateUI(float redPercent, float bluePercent)
    {
        redText.text = $"Red: {redPercent}%";
        blueText.text = $"Blue: {bluePercent}%";

        redSlider.value = redPercent;
        blueSlider.value = bluePercent;
    }
}

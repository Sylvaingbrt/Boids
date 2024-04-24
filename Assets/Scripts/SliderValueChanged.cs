using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueChanged : MonoBehaviour
{
    public enum Type{
        fov,
        separation,
        alignement,
        cohesion,
        detection
    }
    [SerializeField] private TextMeshProUGUI ValueText;
    [SerializeField] private Slider slider;
    [SerializeField] private Type type;


    // Start is called before the first frame update
    void Start()
    {
        float value = GlobalVariables.Instance.GetValue(type);
        slider.value = value;
        //SliderChanged();
    }

    public void SliderChanged(){
        ValueText.text = $"{slider.value:0.##}/{slider.maxValue}";
        GlobalVariables.Instance.ChangeValue(type,slider.value);
    }
}

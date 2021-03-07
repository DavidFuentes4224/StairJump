using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("ForeGrounds")]
    [SerializeField] private Image m_SatForeground;
    [Header("SliderValues")]
    [SerializeField] private Vector3 ColorValue = new Vector3(0, 1, 0.5f);
    
    [Header("Sliders")]
    [SerializeField] private Slider sliderHue;
    [SerializeField] private Slider sliderSat;
    [SerializeField] private Slider sliderBri;
    
    [Header("Refs")]
    [SerializeField] private GameObject AdditionalOptions = null;
    [SerializeField] private TextMeshProUGUI textMesh = null;
    [SerializeField] private AvatarRenderer avatar= null;

    private void Awake()
    {
        AvatarRenderer.PartSelected += OnPartSelected;
    }

    private void OnPartSelected(AvatarRenderer.PartSelectedArgs e)
    {
        ColorValue = Utils.ColorToVector(e.partColor);
        ConfigureSliders(ref e);
    }

    private void Start()
    {
        SetAdditionalOptions(false);

    }

    private void ConfigureSliders(ref AvatarRenderer.PartSelectedArgs e)
    {
        float h, s, v; Color.RGBToHSV(e.partColor, out h, out s, out v);
        sliderBri.value = v;
        sliderHue.value = h;
        sliderSat.value = s;
    }

    public void SetAdditionalOptions(bool enable)
    {
        AdditionalOptions.SetActive(enable);
    }

    public void UpdateForground(float value)
    {
        m_SatForeground.color = Color.HSVToRGB(value, 1, 0.5f);
    }

    public void UpdateSelectedSpriteNumber()
    {
        var number = avatar.GetSelectedSpriteNumber();
        textMesh.text = number.ToString();
    }

    //updating color overlay of saturation bar
    public void UpdateHue(float value) { ColorValue.x = value; }
    public void UpdateSat(float value) { ColorValue.y = value; }
    public void UpdateBri(float value) { ColorValue.z = value; }
}

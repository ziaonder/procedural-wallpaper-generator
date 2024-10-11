using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

public class UIController : MonoBehaviour
{
    [SerializeField] Toggle toggle1, toggle2;
    [SerializeField] Image colorA, colorB;
    Toggle checkedToggle;
    public FlexibleColorPicker fcp;
    public TextMeshProUGUI scaleText;
    public TMP_Dropdown resolution;
    public Slider scaleSlider;
    public static event Action<Color, Color, float, int> OnValueChange;
    public static event Action OnGenerate;
    public GameObject[] previewThings;
    public Image tick;
    public TextMeshProUGUI tickText;

    private void Awake()
    {
        checkedToggle = toggle1;
    }

    private void Start()
    {
        OnValueChange?.Invoke(colorA.color, colorB.color, scaleSlider.value, resolution.value);
    }

    private void Update()
    {
        if(checkedToggle == toggle1 && toggle2.isOn)
        {
            toggle1.isOn = false;
            checkedToggle = toggle2;
        }

        if (checkedToggle == toggle2 && toggle1.isOn)
        {
            toggle2.isOn = false;
            checkedToggle = toggle1;
        }
    }

    public void ChangeColor()
    {
        if(checkedToggle == toggle1)
            colorA.color = fcp.color;
        else
            colorB.color = fcp.color;

        OnValueChange?.Invoke(colorA.color, colorB.color, scaleSlider.value, resolution.value);
    }

    public void ChangeScaleValue()
    {
        scaleText.text = scaleSlider.value.ToString("F3");
        OnValueChange?.Invoke(colorA.color, colorB.color, scaleSlider.value, resolution.value);
    }

    public void ChangeResValue()
    {
        OnValueChange?.Invoke(colorA.color, colorB.color, scaleSlider.value, resolution.value);
    }

    public void OnGenerateButton()
    {
        OnGenerate?.Invoke();
        foreach (GameObject go in previewThings)
            go.SetActive(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnSaveButton()
    {
        ExportImage.textureToSave = TextureGenerator.Instance.Texture;
        ExportImage.Instance.SaveTextureAsPNG();
        StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        tick.enabled = true;
        tickText.enabled = true;

        float timer = 0f;
        while(timer < 1.5f)
        {
            timer += Time.deltaTime;
            tick.color = new Color(tick.color.r, tick.color.g, tick.color.b, Mathf.Lerp(1f, 0f, timer));
            tickText.color = new Color(tickText.color.r, tickText.color.g, tickText.color.b, Mathf.Lerp(1f, 0f, timer));
            yield return null;
        }

        tick.enabled = false;
        tick.color = new Color(tick.color.r, tick.color.g, tick.color.b, 1f);
        tickText.enabled = false;
        tickText.color = new Color(tickText.color.r, tickText.color.g, tickText.color.b, 1f);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class ColorManager : MonoBehaviour
{
    [SerializeField] ColorButton[] buttons = null;
    [Header("Config")]
    [SerializeField] int MAXBUTTONS = 64;
    [SerializeField] Transform colorButtonPrefab = null;

    [Header("Colors")]
    [SerializeField] private ColorPalette skinPalette = null;
    [SerializeField] private ColorPalette mainPalette = null;

    public static event Action<ColorSelectedArgs> ColorSelected = null;
    public class ColorSelectedArgs : EventArgs
    {
        public Color color;
        public ColorSelectedArgs(ByteColor b)
        {
            color = new Color32(b.R,b.G,b.B,255);
        }
    }

    public static void OnColorSelected(ByteColor b)
    {
        ColorSelected?.Invoke(new ColorSelectedArgs(b));
    }

    private void Awake()
    {
        buttons = new ColorButton[MAXBUTTONS];
    }

    private void Start()
    {
        CreateColors();
    }

    private void CreateColors()
    {
        var offset = 50f;
        ResizeRect(MAXBUTTONS);
        for (int i = 0; i < MAXBUTTONS; i++)
        {
            var newColor = Instantiate(colorButtonPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
            newColor.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset, 0);
            buttons[i] = newColor.GetComponent<ColorButton>();
            offset += 200;
        }
    }

    private void UpdateColors(ColorPalette palette)
    {
        var size = palette.Colors.Length;
        ResizeRect(size);
        for(int i = 0; i < size; i++)
        {
            buttons[i].UpdateColor(palette.Colors[i]);
        }
        for(int j = size; j < MAXBUTTONS; j++)
        {
            buttons[j].ClearColor();
        }
    }

    private void ResizeRect(int size)
    {
        var rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(size * 200 + 50, rectTransform.sizeDelta.y);
    }

    public void ChoosePalette(bool isSkin = false)
    {
        if (isSkin)
            UpdateColors(skinPalette);
        else
            UpdateColors(mainPalette);
    }
}

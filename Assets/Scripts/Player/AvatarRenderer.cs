using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRenderer : MonoBehaviour
{
    public enum SELECTEDPART { BODY, EYES, HAIR, BEARD, SHIRT, PANTS,NONE};
    [Header("Sprites")]
    [SerializeField] private Sprite[] m_HairSprites;
    [SerializeField] private Sprite[] m_BeardSprites;

    [Header("Renderers")]
    [SerializeField] private SpriteRenderer m_bodyRenderer;
    [SerializeField] private SpriteRenderer m_eyesRenderer;
    [SerializeField] private SpriteRenderer m_hairRenderer;
    [SerializeField] private SpriteRenderer m_beardRenderer;
    [SerializeField] private SpriteRenderer m_shirtRenderer;
    [SerializeField] private SpriteRenderer m_pantsRenderer;

    [Header("Colors")]
    [SerializeField] private Dictionary<SELECTEDPART,SpriteRenderer> m_SpriteColorByPartName;
    
    [Header("Selectors")]
    [SerializeField] private SELECTEDPART m_selectedPart = SELECTEDPART.BODY;
    [SerializeField] private int m_selectedHair = 0;
    [SerializeField] private int m_selectedBeard = 0;

    public static event Action<PartSelectedArgs> PartSelected = null;

    public class PartSelectedArgs : EventArgs
    {
        public Color partColor;
        public PartSelectedArgs(Color color)
        {
            partColor = color;
        }
    }

    private void Start()
    {
        m_SpriteColorByPartName = new Dictionary<SELECTEDPART, SpriteRenderer>() {
            { SELECTEDPART.BODY, m_bodyRenderer },
            { SELECTEDPART.EYES, m_eyesRenderer},
            { SELECTEDPART.HAIR, m_hairRenderer },
            { SELECTEDPART.BEARD, m_beardRenderer },
            { SELECTEDPART.SHIRT, m_shirtRenderer },
            { SELECTEDPART.PANTS, m_pantsRenderer },
        };
        LoadColors();
        LoadSprites();
    }

    private void LoadSprites()
    {
        var sprites = SaveManager.Instance.GetSelectedSprites();
        m_selectedBeard = sprites.Beard;
        UpdateBeardTexture(m_selectedBeard);
        m_selectedHair = sprites.Hair;
        UpdateHairTexture(m_selectedHair);
    }

    private void LoadColors()
    {
        var colors = SaveManager.Instance.GetColors();
        m_beardRenderer.color = Utils.VectorToColor(colors.Beard);
        m_bodyRenderer.color = Utils.VectorToColor(colors.Body);
        m_hairRenderer.color = Utils.VectorToColor(colors.Hair);
        m_eyesRenderer.color = Utils.VectorToColor(colors.Eyes);
        m_shirtRenderer.color = Utils.VectorToColor(colors.Shirt);
        m_pantsRenderer.color = Utils.VectorToColor(colors.Pants);
    }

    private void UpdateHairTexture(int index)
    {
        m_hairRenderer.sprite = m_HairSprites[index];
    }

    private void UpdateBeardTexture(int index)
    {
        m_beardRenderer.sprite = m_BeardSprites[index];
    }

    public void UpdateSelectedIndex(int value)
    {
        switch(m_selectedPart)
        {
            case (SELECTEDPART.BEARD): { 
                    m_selectedBeard = Mathf.Clamp(m_selectedBeard + value, 0, m_BeardSprites.Length -1);
                    UpdateBeardTexture(m_selectedBeard);
                    break; 
                }
            case (SELECTEDPART.HAIR): {
                    m_selectedHair = Mathf.Clamp(m_selectedHair + value, 0, m_HairSprites.Length - 1);
                    UpdateHairTexture(m_selectedHair);
                    break; 
                }
            default: { break; }
        }
    }

    public void UpdateColor(float value)
    {
        SpriteRenderer spriteRender;
        Debug.Log("updating");
        if (m_SpriteColorByPartName.TryGetValue(m_selectedPart, out spriteRender))
        {
            Debug.Log("found value");
            float h, s, v;
            Color.RGBToHSV(spriteRender.color, out h, out s, out v);
            spriteRender.color = Color.HSVToRGB(value, s, v);

        }
    }

    public void UpdateSatr(float value)
    {
        SpriteRenderer spriteRender;
        Debug.Log("updating");
        if (m_SpriteColorByPartName.TryGetValue(m_selectedPart, out spriteRender))
        {
            Debug.Log("found value");
            float h, s, v;
            Color.RGBToHSV(spriteRender.color, out h, out s, out v);
            spriteRender.color = Color.HSVToRGB(h, value, v);

        }
    }

    public void UpdateBrightness(float value)
    {
        SpriteRenderer spriteRender;
        Debug.Log("updating");
        if (m_SpriteColorByPartName.TryGetValue(m_selectedPart, out spriteRender))
        {
            Debug.Log("found value");
            float h, s, v;
            Color.RGBToHSV(spriteRender.color, out h, out s, out v);
            spriteRender.color = Color.HSVToRGB(h, s, value);

        }
    }

    public void UpdateSelectedPart(int value)
    {
        m_selectedPart = (SELECTEDPART)value;
        var color = m_SpriteColorByPartName[m_selectedPart].color;
        PartSelected?.Invoke(new PartSelectedArgs(color));
    }

    public int GetSelectedSpriteNumber()
    {
        var num = 0;
        switch (m_selectedPart)
        {
            case (SELECTEDPART.BEARD): { num = m_selectedBeard; break; }
            case (SELECTEDPART.HAIR): { num = m_selectedHair; break; }
            default: { break; }
        }
        return num;
    }

    public void SaveSelections()
    {
        SaveManager.Instance.UpdateColors(m_SpriteColorByPartName);
        SaveManager.Instance.UpdateSprites(m_selectedHair, m_selectedBeard);
        SaveManager.Instance.SaveFile();
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "Palette", menuName = "ScriptableObjects/ColorPalette", order = 1)]
public class ColorPalette : ScriptableObject
{
    public string PalletName;
    public ByteColor[] Colors;

    public void UpdateColorPreviews()
    {
        foreach(var c in Colors)
        {
            c.UpdateColor();
        }
    }

    public void SetColors()
    {
        foreach (var c in Colors)
        {
            c.SetColor();
        }
    }
}

[System.Serializable]
public class ByteColor
{
    [SerializeField] private Color32 m_color;
    public byte R;
    public byte G;
    public byte B;

    public void UpdateColor()
    {
        m_color = new Color32(R, G, B,255);
    }

    internal void SetColor()
    {
        R = m_color.r;
        G = m_color.g;
        B = m_color.b;
    }

    public ByteColor()
    {
        UpdateColor();
    }
}

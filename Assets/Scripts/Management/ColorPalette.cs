using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Palette", menuName = "ScriptableObjects/ColorPalette", order = 1)]
public class ColorPalette : ScriptableObject
{
    public string PalletName;
    public ByteColor[] Colors;
}

[System.Serializable]
public class ByteColor
{
    public byte R;
    public byte G;
    public byte B;
}

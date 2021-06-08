using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColorPalette))]
public class ColorPaletteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ColorPalette myPalette = (ColorPalette)target;

        
        if (GUILayout.Button("Update Previews", GUILayout.Height(40)))
        {
            myPalette.UpdateColorPreviews();
        }
        if (GUILayout.Button("Set Colors", GUILayout.Height(40)))
        {
            myPalette.SetColors();
        }
    }
}

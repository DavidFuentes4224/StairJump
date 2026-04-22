using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomizationOptions", menuName = "ScriptableObjects/CustomizationOptions", order = 2)]
public class CustomizationOptions : ScriptableObject
{
	[Header("Sprites")]
	[SerializeField] Sprite[] m_hair;
	[SerializeField] Sprite[] m_breads;
	[Header("Palettes")]
	[SerializeField] ColorPalette m_mainPalette;
	[SerializeField] ColorPalette m_skinPalette;

	public IReadOnlyList<Sprite> Hair => m_hair;
	public IReadOnlyList<Sprite> Beards => m_breads;
	public ColorPalette MainPalette => m_mainPalette;
	public ColorPalette SkinPalette => m_skinPalette;
}
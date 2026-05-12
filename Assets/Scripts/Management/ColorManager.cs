using System;
using UnityEngine;

public sealed class ColorManager : ManagerBase<ColorManager>
{
	[Header("Prefabs")]
	[SerializeField] Transform m_colorButtonPrefab = null;

	[Header("Colors")]
	[SerializeField] private ColorPalette m_skinPalette = null;
	[SerializeField] private ColorPalette m_mainPalette = null;

	public event Action<ColorSelectedArgs> ColorSelected = null;

	public void OnColorSelected(ByteColor b) => ColorSelected?.Invoke(new ColorSelectedArgs(b));

	public void ChoosePalette(bool isSkin = false) => UpdateColors(isSkin ? m_skinPalette : m_mainPalette);

	protected override void Awake()
	{
		base.Awake();
		m_maxButtonLength = Math.Max(m_mainPalette.Colors.Length, m_skinPalette.Colors.Length);
		m_buttons = new ColorButton[m_maxButtonLength];
	}

	private void Start()
	{
		CreateColors();
		UpdateColors(m_skinPalette);
	}

	private void CreateColors()
	{
		var offset = c_buttonSpacing;
		ResizeRect(m_maxButtonLength);
		for (int i = 0; i < m_maxButtonLength; i++)
		{
			var newColor = Instantiate(m_colorButtonPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
			newColor.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset, 0);
			m_buttons[i] = newColor.GetComponent<ColorButton>();
			offset += c_buttonSize;
		}
	}

	private void UpdateColors(ColorPalette palette)
	{
		var numOfColors = palette.Colors.Length;
		ResizeRect(numOfColors);

		for(int i = 0; i < numOfColors; i++)
			m_buttons[i].UpdateColor(palette.Colors[i]);
		for(int j = numOfColors; j < m_maxButtonLength; j++)
			m_buttons[j].ClearColor();
	}

	private void ResizeRect(int numOfColors)
	{
		var rectTransform = gameObject.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(numOfColors * c_buttonSize + c_buttonSpacing, rectTransform.sizeDelta.y);
	}


	const int c_buttonSize = 125;
	const int c_buttonSpacing = 10;
	int m_maxButtonLength;
	ColorButton[] m_buttons;
}

public class ColorSelectedArgs : EventArgs
{
	public ColorSelectedArgs(ByteColor b)
	{
		Color = new Color32(b.R, b.G, b.B, 255);
	}

	public Color Color { get; }
}

using UnityEngine;
using UnityEngine.UI;

public sealed class ColorButton : MonoBehaviour
{
	[SerializeField] ByteColor byteColor = null;
	[SerializeField] Image colorImage = null;

	public ColorButton(ByteColor b)
	{
		byteColor = b;
	}

	public void SelectColor()
	{
		ColorManager.Instance.OnColorSelected(byteColor);
	}

	public void UpdateColor(ByteColor b)
	{
		gameObject.SetActive(true);
		byteColor = b;
		colorImage.color = new Color32(b.R,b.G,b.B,255);
	}

	public void ClearColor()
	{
		gameObject.SetActive(false);
	}
}

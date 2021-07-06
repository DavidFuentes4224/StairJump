using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    [SerializeField] ByteColor byteColor = null;
    [SerializeField] Image colorImage = null;

    public ColorButton(ByteColor b)
    {
        byteColor = b;
    }

    internal void UpdateColor(ByteColor b)
    {
        gameObject.SetActive(true);
        byteColor = b;
        colorImage.color = new Color32(b.R,b.G,b.B,255);
    }

    internal void ClearColor()
    {
        gameObject.SetActive(false);
    }

    public void SelectColor()
    {
        ColorManager.OnColorSelected(byteColor);
    }
}

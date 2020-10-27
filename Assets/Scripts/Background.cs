using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Vector2 m_targetOffset;
    private Renderer m_renderer;
    private void Awake()
    {
        m_renderer = this.GetComponent<Renderer>();
    }

    public float MovementSpeed;
    public float OffsetSpeed;

    // Update is called once per frame
    void Update()
    {
        var offset = Vector2.Lerp(m_renderer.material.GetTextureOffset("_MainTex"), m_targetOffset, Time.deltaTime * MovementSpeed);
        m_renderer.material.SetTextureOffset("_MainTex", offset);
    }

    public void UpdateOffset(int x)
    {
        m_targetOffset += new Vector2(-x, 1) * OffsetSpeed;
    }
}

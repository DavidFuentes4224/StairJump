using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    [Header("Speed Settings")]
    public float GrassSpeed;
    public float Grass2Speed;
    public float CitySpeed;
    public float CloudSpeed;
    public float GlobalSpeed;

    [Header("Speed Settings")]
    public Renderer GrassRenderer;
    public Renderer Grass2Renderer;
    public Renderer CityRenderer;
    public Renderer CloudRenderer;

    [SerializeField]
    private Vector2 m_targetOffset;



    // Start is called before the first frame update
    void Start()
    {
        m_targetOffset = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        var grassOffset = Vector2.Lerp(GrassRenderer.material.GetTextureOffset("_MainTex"), m_targetOffset * GrassSpeed, Time.deltaTime);
        var grass2Offset = Vector2.Lerp(Grass2Renderer.material.GetTextureOffset("_MainTex"), m_targetOffset * Grass2Speed, Time.deltaTime );
        var cityOffset = Vector2.Lerp(CityRenderer.material.GetTextureOffset("_MainTex"), m_targetOffset * CitySpeed, Time.deltaTime );
        var cloudOffset = Vector2.Lerp(CloudRenderer.material.GetTextureOffset("_MainTex"), m_targetOffset * CloudSpeed, Time.deltaTime );

        GrassRenderer.material.SetTextureOffset("_MainTex", grassOffset);
        Grass2Renderer.material.SetTextureOffset("_MainTex", grass2Offset);
        CityRenderer.material.SetTextureOffset("_MainTex", cityOffset);
        CloudRenderer.material.SetTextureOffset("_MainTex", cloudOffset);
    }

    public void UpdateTargetOffset(float direction)
    {
        m_targetOffset += new Vector2(direction, 1) * GlobalSpeed;
    }
}

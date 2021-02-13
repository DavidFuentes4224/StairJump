using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    public float[] LayerSpeeds;
    public Renderer[] Layers;

    [SerializeField]
    private Vector2 m_targetOffset;

    [SerializeField]
    private float globalSpeedModifier = 1f;



    // Start is called before the first frame update
    void Start()
    {
        m_targetOffset = Vector2.zero;
        GameStateManager.RestartGame += OnRestartGame;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < Layers.Length; i++)
        {
            var layer = Layers[i];
            var speed = LayerSpeeds[i];

            var offSet = Vector2.Lerp(layer.material.GetTextureOffset("_MainTex"), m_targetOffset * speed * globalSpeedModifier, Time.deltaTime);
            layer.material.SetTextureOffset("_MainTex", offSet);
        }
        //var grassOffset = Vector2.Lerp(GrassRenderer.material.GetTextureOffset("_MainTex"), m_targetOffset * GrassSpeed * globalSpeedModifier, Time.deltaTime);
        //var grass2Offset = Vector2.Lerp(Grass2Renderer.material.GetTextureOffset("_MainTex"), m_targetOffset * Grass2Speed * globalSpeedModifier, Time.deltaTime );
        //var cityOffset = Vector2.Lerp(CityRenderer.material.GetTextureOffset("_MainTex"), m_targetOffset * CitySpeed * globalSpeedModifier, Time.deltaTime );
        //var cloudOffset = Vector2.Lerp(CloudRenderer.material.GetTextureOffset("_MainTex"), m_targetOffset * CloudSpeed * globalSpeedModifier, Time.deltaTime );

        //GrassRenderer.material.SetTextureOffset("_MainTex", grassOffset);
        //Grass2Renderer.material.SetTextureOffset("_MainTex", grass2Offset);
        //CityRenderer.material.SetTextureOffset("_MainTex", cityOffset);
        //CloudRenderer.material.SetTextureOffset("_MainTex", cloudOffset);
    }

    private void OnRestartGame()
    {
        m_targetOffset = new Vector2 (m_targetOffset.x,0);
    }

    public void UpdateTargetOffset(float direction)
    {
        m_targetOffset += new Vector2(direction, 1) * globalSpeedModifier;
    }
}

using UnityEngine;

public class Parallaxer : MonoBehaviour
{
    public float[] LayerSpeeds;
    public Renderer[] Layers;

    [SerializeField]
    private Vector2 m_targetOffset;

    [SerializeField]
    private float globalSpeedModifier = 1f;

    [SerializeField]
    private bool autoScroll = false;
    [SerializeField]
    private Vector2 autoScrollDirection = new Vector2();

    void Start()
    {
        m_targetOffset = Vector2.zero;
        GameStateManager.RestartGame += OnRestartGame;
        GameStateManager.PlayerJumped += OnPlayerJumped;
    }

    private void OnDestroy()
    {
        GameStateManager.RestartGame -= OnRestartGame;
        GameStateManager.PlayerJumped -= OnPlayerJumped;
    }

    private void OnPlayerJumped(GameStateManager.JumpEventArgs e)
    {
        UpdateTargetOffset(e.Direction);
    }

    void Update()
    {
        for(int i = 0; i < Layers.Length; i++)
        {
            var layer = Layers[i];
            var speed = LayerSpeeds[i];

            var offSet = Vector2.Lerp(layer.material.GetTextureOffset("_MainTex"), m_targetOffset * speed * globalSpeedModifier, Time.deltaTime);
            layer.material.SetTextureOffset("_MainTex", offSet);
        }
        if (autoScroll)
            UpdateTargetOffset(autoScrollDirection);
    }

    private void OnRestartGame()
    {
        m_targetOffset = new Vector2 (m_targetOffset.x,0);
    }

    private void UpdateTargetOffset(float direction)
    {
        m_targetOffset += new Vector2(-direction * Settings.DISTANCE, 1) * globalSpeedModifier;
    }

    private void UpdateTargetOffset(Vector2 direction)
    {
        m_targetOffset += direction * globalSpeedModifier;
    }
}

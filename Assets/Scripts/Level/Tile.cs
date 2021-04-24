using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 Target;
    public float Speed;

    [SerializeField]
    private float coinSpawnChance = 0.25f;
    [SerializeField]
    private GameObject coin = null;
    [SerializeField]
    private Sprite[] textures = null;
    private SpriteRenderer m_renderer = null;

    private void Awake()
    {
        m_renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target, Speed);
    }

    public void UpdateTarget(float x,int y = 1)
    {
        Target += new Vector3(x, -Settings.HEIGHT * y, 0);
    }

    public void ResetTarget()
    {
        Target = transform.position;
    }

    public void TrySpawnCoin()
    {
        var result = Random.Range(0f, 1f);
        if (result < coinSpawnChance)
            coin.SetActive(true);
        else
            coin.SetActive(false);
    }

    public void SetTexture(int height)
    {
        if (height > 100) m_renderer.sprite = textures[3];
        else if (height > 50) m_renderer.sprite = textures[2];
        else if (height > 25) m_renderer.sprite = textures[1];
        else if (height == 0) m_renderer.sprite = textures[4];
        else m_renderer.sprite = textures[0];
    }
}

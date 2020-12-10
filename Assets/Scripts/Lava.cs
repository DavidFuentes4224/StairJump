using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public static Player PlayerRef;
    public float TimeToReact = 3f;
    public float LavaDecreaseAmount = 1f;

    private Vector3 m_originalFirePosition;
    private float m_fireSpeed;

    private void Awake()
    {
        PlayerRef = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerRef.GetIsAlive())
        {
            UpdateFirePosition();
        }
    }

    private void Start()
    {
        m_originalFirePosition = transform.position;
        m_fireSpeed = Vector2.Distance(m_originalFirePosition, PlayerRef.transform.position) / TimeToReact;
    }

    private void UpdateFirePosition()
    {
        //Fire.position += Vector3.up * Time.deltaTime * 2f;
        transform.position += Vector3.up * Time.deltaTime * m_fireSpeed;
    }

    public void DecreaseFirePosition()
    {
        var decreasedPos = transform.position - (Vector3.up * LavaDecreaseAmount);
        var newPos = (decreasedPos.y > m_originalFirePosition.y) ? decreasedPos : m_originalFirePosition;
        transform.position = newPos;
    }
}

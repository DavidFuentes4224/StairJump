using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 Target;
    public float Speed;
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

    public void UpdateTarget(float x)
    {
        Target += new Vector3(x, -Settings.HEIGHT, 0);
    }

    public void ResetTarget()
    {
        Target = transform.position;
    }
}

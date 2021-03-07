using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ColorToVector(Color color)
    {
        return new Vector3(color.r, color.g, color.b);
    }

    public static Color VectorToColor(Vector3 vector)
    {
        return new Color(vector.x, vector.y, vector.z);
    }
}

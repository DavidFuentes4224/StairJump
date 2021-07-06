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

    public static Vector3 FlipLocalScale(Vector3 currentScale)
    {
        return new Vector3(-currentScale.x, currentScale.y, currentScale.z);
    }

    public static Vector3 SetLocalScale(Vector3 currentScale,int direction)
    {
        return new Vector3(currentScale.x * direction, currentScale.y, currentScale.z);
    }
}

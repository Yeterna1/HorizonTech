
using UnityEngine;

public static class MathTool {
    public static bool IsSameDirection(Vector3 dir1, Vector3 dir2, float threshold = 0.5f) {
        return Vector3.Dot(dir1.normalized, dir2.normalized) > threshold;
    }
}


using System;
using UnityEngine;

public class LightDir : MonoBehaviour  {
    private void LateUpdate() {
        Quaternion rotation = Quaternion.Euler(90.0f - transform.parent.rotation.x, 0.0f, 0.0f);
        transform.rotation = rotation;
    }
}

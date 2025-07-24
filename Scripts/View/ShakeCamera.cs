using System.Collections;
using UnityEngine;

public class ShakeCamera : MonoBehaviour{

    private bool IsShake = false;

    private IEnumerator ShakeCoroutine(float duration, float magnitude, float late){
        IsShake = true;
        yield return new WaitForSeconds(late);
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration){
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(originalPos.x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        IsShake = false;
    }

    public void Shake(float duration, float magnitude, float late = 0.0f){
        if (IsShake) return;
        StartCoroutine(ShakeCoroutine(duration, magnitude, late));
    }

}

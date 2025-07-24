using UnityEngine;

public class FollowCamera : MonoBehaviour {

    private Transform FollowTarget;

    public void SetFollowTarget(Transform target) {
        this.FollowTarget = target;
    }

    private void LateUpdate() {
        if (!this.FollowTarget) return;
        Vector3 pos = this.transform.position;
        pos.x = this.FollowTarget.position.x;
        this.transform.position = pos;
    }
}

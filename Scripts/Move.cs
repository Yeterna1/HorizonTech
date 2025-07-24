using System;
using UnityEngine;

public class Move : MonoBehaviour{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public Transform RendererTransform { get; private set; }
    
    private BoxCollider Collider;
    public Animator MoveAnimator { get; private set; }

    public bool IsMove { get; private set; }

    private void Awake(){
        Collider = GetComponent<BoxCollider>();
        MoveAnimator = GetComponentInChildren<Animator>();
    }

    public void ChangeForward(float sign) {
        float scaleX = RendererTransform.localScale.x;
        if (sign > 0.0f) {
            scaleX = -Mathf.Abs(scaleX);
        } else if (sign < 0.0f) {
            scaleX = Mathf.Abs(scaleX);
        }
        RendererTransform.localScale = new Vector3(scaleX, 
            RendererTransform.localScale.y, RendererTransform.localScale.z);
    }
    
    // TODO: Auto Find Way -> NavMeshAgent / FlowField
    public void MoveByDirection(Vector3 velocity) {
        
        MoveAnimator.SetFloat(AnimationParams.Velocity, velocity.sqrMagnitude);
        if (velocity == Vector3.zero){
            return;
        }

        //  Control Character Face Forward
        ChangeForward(velocity.x);
        
        // Check Collision
        // BUG: Collision Detect Error
        bool isCollision = Physics.BoxCast(RendererTransform.position, Collider.size * 0.7f, 
            velocity, transform.rotation, Speed * Time.deltaTime, 
            LayerMask.GetMask(LayerName.Building, LayerName.Enemy));
        
        if (!isCollision){
            this.transform.position += Speed * Time.deltaTime * velocity;    
        }
    }

    private void Update(){
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 velocity = new Vector3(x, 0, y).normalized;
        IsMove = velocity != Vector3.zero; 
        MoveByDirection(velocity);
    }
}

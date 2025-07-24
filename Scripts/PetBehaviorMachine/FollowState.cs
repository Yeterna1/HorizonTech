
using UnityEngine;

public class FollowState : PetState{

    private Transform FollowTarget;
    private Vector3 MoveVec => FollowTarget.position - transform.position;
    
    public override void Construct(){
        this.FollowTarget = Controller.FollowTarget;
    }

    public override void Execute(){
        Controller.PetMove.MoveByDirection(MoveVec.normalized);
        // this.transform.position += Controller.MoveSpeed * Time.deltaTime * MoveVec.normalized;
    }

    public override void Transition(){
        float distance = GlobalDataManager.Instance.PetStopDistance;
        if (MoveVec.sqrMagnitude < distance * distance){
            Controller.ChangeState(null);
        }
    }
}

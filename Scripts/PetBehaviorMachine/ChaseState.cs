
using UnityEngine;

public class ChaseState : PetState{

    private Transform AttackTarget;
    private Vector3 MoveVec => AttackTarget ? AttackTarget.position - transform.position : Vector3.zero;

    private AttackState PetAttack;

    protected override void Awake(){
        base.Awake();
        PetAttack = GetComponent<AttackState>();
    }

    public override void Construct(){
        this.AttackTarget = Controller.AttackTarget.transform;
    }

    public override void Execute(){
        if (!this.AttackTarget) return;
        Controller.PetMove.MoveByDirection(MoveVec.normalized);
        // this.transform.position += Controller.MoveSpeed * Time.deltaTime * MoveVec.normalized;
    }
    
    public override void Transition(){
        if(!AttackTarget) Controller.ChangeState(null);
        float distance = Controller.Data.AttackRadius;
        if (MoveVec.sqrMagnitude <= distance * distance){
            Controller.ChangeState(PetAttack);
        }
    }
}

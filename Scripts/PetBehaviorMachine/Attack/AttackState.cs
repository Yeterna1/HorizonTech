
using UnityEngine;

public abstract class AttackState : PetState{

    [SerializeField] protected ParticleSystem AttackParticle;
    
    protected Enemy AttackTarget;
    
    // protected Vector3 MoveVec => AttackTarget ? AttackTarget.position - transform.position : Vector3.zero;

    private ChaseState PetChase;
    private SkillState PetSkill;

    private float[] LastSkillTime;
    private float LastAttackTime;
    

    protected override void Awake(){
        base.Awake();
        PetChase = GetComponent<ChaseState>();
        PetSkill = GetComponent<SkillState>();

        LastSkillTime = new float[Controller.Data.PetSkills.Length];
    }

    public override void Construct(){
        this.AttackTarget = Controller.AttackTarget;
        // TODO: DELETE
        Controller.PetMove.MoveAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
    }

    public override void Destruct() {
        if (AttackParticle) {
            AttackParticle.Stop();
        }
    }

    public override void Execute(){
        if (!this.AttackTarget) return;

        Vector3 moveVec = AttackTarget.Aim.position - transform.position;
        Controller.PetMove.ChangeForward(moveVec.x);

        float coolDownTime = Controller.Data.AutoAttackCooldownTime;
        if (LastAttackTime > 0.0f && Time.time - LastAttackTime < coolDownTime) return;
        Attack();
        LastAttackTime = Time.time;
    }

    protected abstract void Attack();

    public override void Transition(){
        if (!AttackTarget || !AttackTarget.Aim) {
            Controller.ChangeState(null);
            return;
        }

        float distance = Controller.Data.AttackRadius;
        if ((AttackTarget.Aim.position - transform.position).sqrMagnitude > distance * distance){
            Controller.ChangeState(PetChase);
            return;
        }
        
        int skillCount = Controller.Data.PetSkills.Length;
        if (Controller.IsPlayerChoose && skillCount != 0){
            for (int i = 0; i < skillCount; i++) {
                float skillCooldownTime = Controller.Data.PetSkills[i].CooldownTime;
                if (Input.GetKeyDown(KeyCode.K + i) &&
                    (LastSkillTime[i] == 0.0f || Time.time - LastSkillTime[i] > skillCooldownTime)){
                    LastSkillTime[i] = Time.time;
                    PetSkill.SetCastSkill(i);
                    Controller.ChangeState(PetSkill);
                    break;
                }
            }
        }
    }
}

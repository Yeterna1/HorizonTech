using UnityEngine;

public class Pet : MonoBehaviour{

    [field: SerializeField] public PetData Data { get; private set; }
    [field: SerializeField] public Transform PetCenter { get; private set; }
    [SerializeField] private int PetIndex;
    [SerializeField] private GameObject Choose;
    
    private PetState CurrentPetState;
    
    public Transform FollowTarget { get; private set; }
    public Enemy AttackTarget { get; private set; }

    private ChaseState PetChase;
    private FollowState PetFollow;
    public bool IsPlayerChoose{ get; private set; }

    public Move PetMove { get; private set; }

    private void Awake(){
        PetChase = GetComponent<ChaseState>();
        PetFollow = GetComponent<FollowState>();
        PetMove = GetComponent<Move>();
        PetMove.enabled = false;
    }

    private void Start() {
        FollowTarget = this.PetIndex == 0 ? GameManager.Instance.Player.transform :
            this.transform.parent.GetChild(this.PetIndex - 1);
        GameManager.Instance.Player.OnAttackEnemy += OnAttack;
        GameManager.Instance.Player.OnSwitchPet += OnSwitchPet;
    }

    private void OnSwitchPet(int index) {
        this.IsPlayerChoose = index == PetIndex;
        PetMove.enabled = this.IsPlayerChoose;
        Choose.SetActive(this.IsPlayerChoose);
        if (!this.IsPlayerChoose) return;
        GameManager.Instance.SetCameraFollowTarget(this.transform);
    }

    private void OnAttack(Enemy attackTarget) {
        if (!attackTarget) return;
        this.AttackTarget = attackTarget;
        this.AttackTarget.OnDead += OnTargetEnemyDead;
        ChangeState(this.PetChase);
    }

    private void OnTargetEnemyDead() {
        this.AttackTarget.OnDead -= OnTargetEnemyDead;
        this.AttackTarget = null;
        ChangeState(this.PetFollow);
    }

    private void Update() {
        if (PetMove.IsMove) return;
        float distance = PetMove.Speed * 0.6f;
        if (!CurrentPetState && 
            (this.transform.position - FollowTarget.position).sqrMagnitude > distance * distance){
            ChangeState(this.PetFollow);
        }

        if (!CurrentPetState) {
            PetMove.MoveAnimator.SetFloat(AnimationParams.Velocity, 0.0f);
            return;
        }
        CurrentPetState.Transition();
        if(CurrentPetState) CurrentPetState.Execute();
    }

    public void ChangeState(PetState newState){
        if(CurrentPetState) CurrentPetState.Destruct();
        CurrentPetState = newState;
        if (CurrentPetState) CurrentPetState.Construct();
        if (CurrentPetState && CurrentPetState == PetFollow) {
            PetMove.enabled = false;
            Choose.SetActive(false);
        }
    }
}



using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour{
    
    private Camera MainCamera;
    private Enemy CurrentAttackTarget;
    private Move PlayerMove;

    private bool IsAttack;

    public Action<Enemy> OnAttackEnemy;
    public Action<Transform> OnChooseEnemy;
    public Action<int> OnSwitchPet;
    
    private void Awake(){
        MainCamera = Camera.main;    
        PlayerMove = GetComponent<Move>();
        PlayerMove.enabled = true;
    }

    private void Update(){
        if (Input.GetMouseButtonDown(0)){
            ChooseTarget();
        }
        
        if (!IsAttack){
            if (Input.GetKeyDown(KeyCode.J)){
                AttackTarget();
            }
        } else if (Input.GetKeyDown(KeyCode.C)) {
            SwitchPet(-1);
            PlayerMove.enabled = true;
            GameManager.Instance.SetCameraFollowTarget(this.transform);
        } else {
            int petCount = GameManager.Instance.CurrentPetCount;
            for (int i = 0; i < petCount; i++) {
                if (!Input.GetKeyDown(KeyCode.Alpha1 + i) && !Input.GetKeyDown(KeyCode.Keypad1 + i)) continue;
                SwitchPet(i);
                PlayerMove.enabled = false;
                break;
            }
        }
    }
    
    private void ChooseTarget() {
        // TODO : TEMP -> Cant Switch Attack Target When Attack
        if (IsAttack && CurrentAttackTarget) return;
        Ray screenRay = MainCamera.ScreenPointToRay(Input.mousePosition);
        this.CurrentAttackTarget = null;
        if (Physics.Raycast(screenRay, out RaycastHit hit, float.MaxValue, LayerMask.GetMask(LayerName.Enemy))
            && hit.transform.gameObject.TryGetComponent(out Enemy enemy)) {
            this.CurrentAttackTarget = enemy;
        }
        OnChooseEnemy?.Invoke(this.CurrentAttackTarget ? this.CurrentAttackTarget.transform : null);
    }

    private void AttackTarget(){
        if (!this.CurrentAttackTarget) return;
        IsAttack = true;
        OnAttackEnemy?.Invoke(this.CurrentAttackTarget);
        this.CurrentAttackTarget.OnDead += OnTragetEnemyDead;
    }

    private void OnTragetEnemyDead() {
        this.CurrentAttackTarget.OnDead -= OnTragetEnemyDead;
        this.CurrentAttackTarget = null;
        IsAttack = false;
        PlayerMove.enabled = true;
        GameManager.Instance.SetCameraFollowTarget(this.transform);
    }

    private void SwitchPet(int index){
        OnSwitchPet?.Invoke(index);
    }
}

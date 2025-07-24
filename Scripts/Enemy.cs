
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour{

    [SerializeField] private EnemyData Data;
    [field: SerializeField] public Transform Aim { get; private set; }
    
    [Header("UI")] 
    [SerializeField] private Canvas UICanvas;
    [SerializeField] private Image BloodBar;
    [SerializeField] private GameObject SelectedImage;
    [SerializeField] private DamageUI DamageUIPrefab;
    
    private float CurrentHealth;

    private bool Invincible;
    private WaitForSeconds InvincibleTimer;

    private Animator EnemyAnimator;

    public Action OnDead;
    
    private void Awake(){
        // this.GetComponentInChildren<Renderer>().material = this.Data.EnemyMaterial;
        this.CurrentHealth = Data.Health;
        EnemyAnimator = GetComponentInChildren<Animator>();
    }

    private void Start(){
        GameManager.Instance.Player.OnChooseEnemy += Selected;
        this.InvincibleTimer = new WaitForSeconds(GlobalDataManager.Instance.EnemyBeAttackedCoolDownTime);
    }

    private void OnDestroy(){
        GameManager.Instance.Player.OnChooseEnemy -= Selected;
    }

    private void Selected(Transform target){
        SelectedImage.SetActive(target == this.transform);
    }

    public void BeAttacked(DamageMessage msg){
        if (Invincible) return;
        StartCoroutine(BeAttackedCoroutine(msg)); 
    }

    private void ShowDamage(float damage) {
        DamageUI ui = Instantiate(DamageUIPrefab, UICanvas.transform);
        RectTransform rectTrans = (RectTransform)ui.transform;
        Vector3 anchoredPos = rectTrans.anchoredPosition;
        anchoredPos += Aim.localPosition;
        rectTrans.anchoredPosition = anchoredPos;
        ui.Show(damage);
    }

    private IEnumerator BeAttackedCoroutine(DamageMessage msg){
        Invincible = true;
        EnemyAnimator.SetTrigger(AnimationParams.Hit);
        ShowDamage(msg.Damage);
         
        this.CurrentHealth -= msg.Damage;
        BloodBar.fillAmount = this.CurrentHealth / this.Data.Health;
        
        if (this.CurrentHealth <= 0.0f){
            UICanvas.gameObject.SetActive(false);
            EnemyAnimator.SetTrigger(AnimationParams.Dead);
            OnDead?.Invoke();
            Destroy(this.gameObject, 2.0f);
            yield break;
        }
        
        yield return this.InvincibleTimer;
        Invincible = false;
    }
}

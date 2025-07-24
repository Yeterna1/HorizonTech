
using UnityEngine;

public class SkillParticle : MonoBehaviour{
    
    private ParticleSystem Particle;
    private Collider ParticleCollider;

    private DamageMessage SkillDamageMsg;

    private void Awake(){
        Particle = this.GetComponent<ParticleSystem>();
        ParticleCollider = this.GetComponent<Collider>();
        ParticleCollider.enabled = false;
    }

    public void Play(DamageMessage msg) {
        this.SkillDamageMsg = msg;
        Particle.Play();
        ParticleCollider.enabled = true;
    }

    private void OnParticleSystemStopped(){
        ParticleCollider.enabled = false;
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.TryGetComponent(out Enemy enemy)) {
            enemy.BeAttacked(new DamageMessage{
                Damage = SkillDamageMsg.Damage,
                Force = 0.0f,
            });
        }
    }
}

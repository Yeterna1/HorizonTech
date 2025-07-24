
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SkillData", menuName = "PhantomSpirit/Skill")]
public class SkillData : ScriptableObject {
    public float Damage;
    public float CooldownTime;
    public SkillParticle skillParticlePrefab;
    public bool IsCastInEnemyLocation = false;
    public bool IsAttackEveryEnemy = false;
}



using UnityEngine;

[CreateAssetMenu(fileName = "PetData", menuName = "PhantomSpirit/Pet")]
public class PetData : ScriptableObject{
    public int CurrentLevel = 1;
    public float AttackRadius;
    public float Damage;
    public float Force;
    public float AutoAttackCooldownTime;
    public SkillData[] PetSkills;
    public PetData NextLevelPetData;
};

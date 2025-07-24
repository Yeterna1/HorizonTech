
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SkillData", menuName = "PhantomSpirit/Skill")]
public class SkillData : ScriptableObject
{
    public int id;
    public string skillName;
    public TargetType targetType;
    public float cooldownTime = 0;

}


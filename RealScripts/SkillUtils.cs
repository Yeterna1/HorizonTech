using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUtils : MonoBehaviour
{
    public Skill CreateSkill()
    {
        return new Skill();
    }
    
}

public class SkillDebugger
{
    
    
    public void DebugDigit()
    {
        Debug.Log(0);
    }
    
}

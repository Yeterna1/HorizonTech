using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill : MonoBehaviour
{
    // Properties
    private float lastStartCastTime;
    
    private bool isCooldown;
    private bool canCast;
    private bool isCasting;
    
    // private Entity enemy 

    private SkillData skillData;
    private SkillDelivery skillDelivery;

    public Skill()
    {
        lastStartCastTime=0;
        isCooldown = true;
        canCast = false;
    }
    
    // Methods
    private bool GetTarget()
    {
        return true;
    }

    private void TargetValid(){}
    
    private void OnSkillCast(){}

    private bool IsCooldown()
    {
        if (Time.deltaTime - lastStartCastTime > skillData.cooldownTime)
        {
            lastStartCastTime = Time.deltaTime;
            return true;
        }
        return false;
    }

    private void Update()
    {
        if(IsCooldown()) isCooldown = true;
        throw new NotImplementedException();
    }
    
    
    
    
}

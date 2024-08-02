using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class FSM_ZombieAttack : FSM_State
{
    [NonSerialized] public FSM_ZombieControl parent;
    public override void Enter()
    {
        base.Enter();
        Debug.LogError(" Enter attack state");
        parent.agent.isStopped = true;
        parent.zombieDataBinding.Speed = 0;
    }
    public override void Update()
    {
        base.Update();
        if (parent.time_count_attack >= parent.attack_speed)
        {
            parent.zombieDataBinding.Attack = true;
            parent.time_count_attack = 0;
        }
    }
    public override void OnAnimMiddle()
    {
        float dis = Vector3.Distance(parent.trans.position, parent.characterControl.trans.position);
        Vector3 dir = parent.characterControl.trans.position - parent.trans.position;
        float dot = Vector3.Dot(dir.normalized, parent.trans.forward);

        if (dot > parent.dot_attack && dis <= parent.range_Attack)
            MissionManager.instance.OnDamage(parent.damageData);
    }
    public override void OnAnimExit()
    {
        float dis = Vector3.Distance(parent.trans.position, parent.characterControl.trans.position);
        Vector3 dir = parent.characterControl.trans.position - parent.trans.position;
        float dot = Vector3.Dot(dir.normalized, parent.trans.forward);

        if (dot > parent.dot_attack && dis <= parent.range_Attack)
        {

        }
        else if (dot > parent.dot_attack && dis <= parent.range_Detect)
            parent.GotoState(parent.chase);
        else
            parent.GotoState(parent.wander);
    }
    public override void Exit()
    {
        base.Exit();
        Debug.LogError(" Exit attack state");
    }
}

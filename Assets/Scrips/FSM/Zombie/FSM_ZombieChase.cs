using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class FSM_ZombieChase : FSM_State
{
    [NonSerialized] public FSM_ZombieControl parent;
    public float speed_move = 2;
    float time_Delay;
    public override void Enter()
    {
        base.Enter();
        time_Delay = 0;
        parent.agent.enabled = true;
        parent.agent.speed = 2;
        parent.agent.Warp(parent.trans.position);
        parent.agent.isStopped = false;
        parent.agent.stoppingDistance = parent.range_Attack - 0.1f;
    }
    public override void Update()
    {
        time_Delay += Time.deltaTime;
        Vector3 cur_point = parent.characterControl.trans.position;

        parent.agent.SetDestination(cur_point);
        float dis = Vector3.Distance(parent.trans.position, cur_point);
        if (parent.agent.remainingDistance <= parent.range_Attack && time_Delay > 0.2f)
            parent.GotoState(parent.zombieAttack);
        else
        {
            parent.RotateAgent();
            parent.zombieDataBinding.Speed = parent.agent.velocity.magnitude;
        }
    }
    public override void Exit()
    {
        base.Exit();
        parent.agent.isStopped = true;
        parent.zombieDataBinding.Speed = 0;
        parent.agent.stoppingDistance = 0;
    }
}

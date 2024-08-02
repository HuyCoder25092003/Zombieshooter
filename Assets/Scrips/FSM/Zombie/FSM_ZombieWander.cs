using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FSM_ZombieWander : FSM_State
{
    [NonSerialized] public FSM_ZombieControl parent;
    float time_Delay;
    Vector3 cur_point;
    Vector3 GetPoint()
    {
        time_Delay = 0;
        Vector2 p = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(2f, 10f);
        return parent.trans.position + new Vector3(p.x, 0, p.y);
    }
    public override void Enter()
    {
        cur_point = GetPoint();
        parent.agent.enabled = true;
        parent.agent.Warp(parent.trans.position);
        parent.agent.isStopped = false;
        parent.agent.speed = 1;
    }
    public override void FixedUpdate()
    {
        time_Delay += Time.deltaTime;
        parent.agent.SetDestination(cur_point);
        float dis = Vector3.Distance(parent.trans.position, cur_point);
        if (parent.agent.remainingDistance <= 0.1f && time_Delay > 0.5f)
        {
            cur_point = GetPoint();
        }
        parent.RotateAgent();

        parent.zombieDataBinding.Speed = parent.agent.velocity.magnitude;
    }
    public override void Exit()
    {
        base.Exit();
        parent.zombieDataBinding.Speed = 0;
        parent.agent.isStopped = true;
    }
}

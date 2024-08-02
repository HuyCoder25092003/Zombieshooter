using SWS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FSM_ZombieStart : FSM_State 
{
    [NonSerialized] public FSM_ZombieControl parent;
    public override void Enter()
    {
        base.Enter();
        parent.zombieDataBinding.Speed = 1;
        parent.splineMove_.StartMove();
        parent.splineMove_.movementEndEvent += MovementEndEvent;
    }
    void MovementEndEvent()
    {
        parent.GotoState(parent.wander);
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        parent.splineMove_.movementEndEvent -= MovementEndEvent;
        parent.splineMove_.Stop();
        parent.zombieDataBinding.Speed = 0;
    }
}

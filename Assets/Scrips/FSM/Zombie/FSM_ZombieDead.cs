using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FSM_ZombieDead : FSM_State
{
    [NonSerialized] public FSM_ZombieControl parent;
    public override void Enter()
    {
        parent.agent.enabled = false;
        parent.zombieDataBinding.Died = true;
        parent.OnDead();
    }
}

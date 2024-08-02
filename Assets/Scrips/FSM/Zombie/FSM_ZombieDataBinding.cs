using RootMotion.Dynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_ZombieDataBinding : MonoBehaviour
{
    public Animator anim;
    int Anim_Key_Speed;
    public PuppetMaster puppetMaster;
    public PuppetMaster.StateSettings stateSettings = PuppetMaster.StateSettings.Default;
    public float Speed
    {
        set
        {
            anim.SetFloat(Anim_Key_Speed, value);
        }
    }
    public bool Attack
    {
        set
        {
            anim.CrossFade("Attack", 0, 0);
        }
    }
    public bool Died
    {
        set
        {
            if (value)
            {
                puppetMaster.mode = PuppetMaster.Mode.Active;
                puppetMaster.Kill(stateSettings);
            }
        }
    }
    void Awake()
    {
        Anim_Key_Speed = Animator.StringToHash("Speed");
    }
}

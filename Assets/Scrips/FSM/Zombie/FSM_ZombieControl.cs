using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_ZombieControl : EnemyControl
{
    public FSM_ZombieAttack zombieAttack;
    public FSM_ZombieChase chase;
    public FSM_ZombieStart start;
    public FSM_ZombieDataBinding zombieDataBinding;
    public FSM_ZombieWander wander;
    public FSM_ZombieDead dead;
    public float dot_attack = 0.5f;
    public float speed_move = 0.8f;
    public override void Setup(EnemyInitData enemyInitData)
    {
        base.Setup(enemyInitData);
        zombieAttack.parent = chase.parent = start.parent = wander.parent = dead.parent = this;
        agent.updateRotation = false;
        GotoState(start);
        StartCoroutine("LoopCheckAttack");
    }
    public void RotateAgent()
    {
        Vector3 dir = agent.steeringTarget - trans.position;
        dir.Normalize();
        if (dir != Vector3.zero)
        {
            Quaternion q = Quaternion.LookRotation(dir, Vector3.up);
            trans.localRotation = Quaternion.Slerp(trans.localRotation, q, Time.deltaTime * 120);
        }
    }
    IEnumerator LoopCheckAttack()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            Vector3 target_character = characterControl.trans.position;

            float dis = Vector3.Distance(trans.position, target_character);
            Vector3 dir = target_character - trans.position;
            dir.Normalize();
            float dot = Vector3.Dot(trans.forward, dir);
            if (dis <= range_Detect && dot > dot_attack)
            {
                if (cur_State == wander || cur_State == start)
                    GotoState(chase);
            }
        }
    }
    public override void OnDamageBullet(Bulletdata bulletdata)
    {
        this.bulletdata = bulletdata;
        Debug.LogError("Bullet data: " + bulletdata.damage);
        hp -= this.bulletdata.damage;
        if (hp <= 0 && cur_State != dead)
        {
            GotoState(dead);
            Invoke("ImpactPhysic", 0.1f);
        }
        base.OnDamageBullet(this.bulletdata);
    }
    public override void OnDamageGrenade(Grenadedata grenadedata)
    {
        Debug.LogError("Bullet data: " + grenadedata.damage);
        hp -= grenadedata.damage;
        if (hp <= 0 && cur_State != dead)
            GotoState(dead);
        base.OnDamageGrenade(grenadedata);
    }
    void ImpactPhysic()
    {
        bulletdata.rig_body.AddForceAtPosition(bulletdata.force, bulletdata.point_impact, ForceMode.Impulse);
    }
}

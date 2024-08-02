using SWS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyInitData
{
    public ConfigEnemyRecord cf;
}
public class DamageData
{
    public int damage;
}
public class EnemyControl : FSM_System
{
    protected Bulletdata bulletdata;
    public int hp, max_hp;
    public DamageData damageData = new DamageData();
    public ConfigEnemyRecord cf;
    public NavMeshAgent agent;
    public Transform trans;
    public splineMove splineMove_;
    public GameObject enemy_detect;
    public float time_count_attack = 0;
    public float range_Detect;
    public float range_Attack; 
    public float attack_speed;
    public CharacterControl characterControl;
    HPHub hpHub;
    public Transform anchor_hub;
    public bool isAlive = true;
    private void Awake()
    {
        trans = transform;
        GameObject char_object = GameObject.FindGameObjectWithTag("Player");
        characterControl = char_object.GetComponent<CharacterControl>();
    }
    public virtual void Setup(EnemyInitData enemyInitData)
    {
        int index = 0;
        trans.position = SceneConfig.instance.GetPointSpawn(out index).position;
        int min = index * 3 +1;
        int max = min + 3;
        string path = "Path_"+UnityEngine.Random.Range(min, max).ToString();
        splineMove_.pathContainer = WaypointManager.Paths[path];
        cf = enemyInitData.cf;
        hp = max_hp = cf.HP;
        damageData.damage = cf.Damage;

        Transform hub_trans = BYPoolManager.instance.GetPool("HpHub").Spawn();
        IngameView ingameView = (IngameView)ViewManager.instance.cur_view;
        hub_trans.transform.SetParent(ingameView.parent_hub, false);
        hpHub = hub_trans.GetComponent<HPHub>();
        hpHub.SetUp(anchor_hub, ingameView.parent_hub, Color.red);
        hpHub.UpdateHP(hp, cf.HP);

    }
    public void OnDead()
    {
        isAlive = false;
        hpHub.OnDetachHub();
        Destroy(enemy_detect);
        MissionManager.instance.EnemyDead(this);
        Invoke("DelayDestroy", 3);
    }
    void DelayDestroy()
    {
        Destroy(gameObject);
    }
    protected override void Update()
    {
        base.Update();
        time_count_attack += Time.deltaTime;

        Vector3 pos_ch = characterControl.trans.position;
        pos_ch.y = trans.position.y;
        float dis = Vector3.Distance(pos_ch, trans.position);
        if (enemy_detect != null)
            enemy_detect.SetActive(dis <= 10);
    }
    public virtual void OnDamageBullet(Bulletdata bulletdata)
    {
        hpHub.UpdateHP(hp, max_hp);
    }
    public virtual void OnDamageGrenade(Grenadedata grenadedata)
    {
        hpHub.UpdateHP(hp, max_hp);
    }
}

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ConfigEnemyRecord
{
    //id	name	prefab	damage	hp	skill
    public int id;
    [SerializeField]
    private string name;
    public string Name
    {
        get
        {
            return name;
        }
    }
    [SerializeField]
    private string prefab;
    public string Prefab
    {
        get
        {
            return prefab;
        }
    }
    [SerializeField]
    private int damage;
    public int Damage
    {
        get
        {
            return damage;
        }
    }
    [SerializeField]
    private int hp;
    public int HP
    {
        get
        {
            return hp;
        }
    }
    [SerializeField]
    private bool skill;
    public bool Skill
    {
        get
        {
            return skill;
        }
    }
}
public class ConfigEnemy : BYDataTable<ConfigEnemyRecord>
{
    public override ConfigCompare<ConfigEnemyRecord> DefindCompare()
    {
        configCompare = new ConfigCompare<ConfigEnemyRecord>("id");
        return configCompare;
    }
}
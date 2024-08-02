using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WeaponType
{
    Assault = 1,
    Handgun = 2,
    Shotgun = 3,
    Machine = 4,
    Special = 5
}
[Serializable]
public class ConfigWeaponRecord
{
    public int id;
    [SerializeField]
    private WeaponType weaponType;
    public WeaponType WP_Type
    {
        get
        {
            return weaponType;
        }
    }
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
    private float rof;
    public float ROF
    {
        get
        {
            return rof;
        }
    }
    [SerializeField]
    private int range;
    public int Range
    {
        get
        {
            return range;
        }
    }
    [SerializeField]
    private int damge;
    public int Damge
    {
        get
        {
            return damge;
        }
    }
    [SerializeField]
    private int accuracy_min;
    public int Accuracy_min
    {
        get
        {
            return accuracy_min;
        }
    }
    [SerializeField]
    private int accuracy_max;
    public int Accuracy_max
    {
        get
        {
            return accuracy_max;
        }
    }
    [SerializeField]
    private int reload;
    public int Reload
    {
        get
        {
            return reload;
        }
    }
}
public class ConfigWeapon : BYDataTable<ConfigWeaponRecord>
{
    public override ConfigCompare<ConfigWeaponRecord> DefindCompare()
    {
        configCompare = new ConfigCompare<ConfigWeaponRecord>("id");
        return configCompare;
    }
    public List<ConfigWeaponRecord> GetRecordBuyWeaponType(WeaponType wp_type)
    {
        return records.Where(x => x.WP_Type == wp_type).ToList();
    }
}

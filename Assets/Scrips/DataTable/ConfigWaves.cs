using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfigWaveRecord
{
    public int id;
    [SerializeField]
    private string enemies;
    public List<int> Enemies
    {
        get
        {
            List<int> ls = new List<int>();
            string[] s_array = enemies.Split(':');
            foreach (string e in s_array)
            {
                ls.Add(int.Parse(e));
            }
            return ls;
        }
    }
    [SerializeField]
    private int delay;
    public int Delay
    {
        get
        {
            return delay;
        }
    }
    [SerializeField]
    private int total;
    public int Total
    {
        get
        {
            return total;
        }
    }
    [SerializeField]
    private int time_rate; // thời gian tạo enemy mới
    public int Time_rate
    {
        get
        {
            return time_rate;
        }
    }

}
public class ConfigWaves : BYDataTable<ConfigWaveRecord>
{
    public override ConfigCompare<ConfigWaveRecord> DefindCompare()
    {
        configCompare = new ConfigCompare<ConfigWaveRecord>("id");
        return configCompare;
    }
}

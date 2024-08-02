using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfigMissionRecord
{
    [SerializeField] int id;
    public int ID => id;
    [SerializeField] string sceneName;
    public string SceneName => sceneName;

    [SerializeField] string waves;
    public List<int> Waves
    {
        get
        {
            string[] s = waves.Split(';');
            List<int> ls = new List<int>();
            foreach (string e in s)
            {
                ls.Add(int.Parse(e));
            }
            return ls;
        }
    }
}
public class ConfigMission : BYDataTable<ConfigMissionRecord>
{
    public override ConfigCompare<ConfigMissionRecord> DefindCompare()
    {
        configCompare = new ConfigCompare<ConfigMissionRecord>("id");
        return configCompare;
    }
}
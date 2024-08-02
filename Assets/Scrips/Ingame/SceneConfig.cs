using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneConfig : BYSingletonMono<SceneConfig>
{
    public Transform player_pos;
    [SerializeField] List<Transform> point_spwans;
    [SerializeField] List<Transform> point_ogres;
    public Transform GetPointSpawn(out int index)
    {
        index = UnityEngine.Random.Range(0, point_spwans.Count);
        return point_spwans[index];
    }
    public Transform GetPointMove()
    {
        return point_ogres.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
    }
}

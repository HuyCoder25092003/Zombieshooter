using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BodyType
{
    HEAD = 1,
    NORMAL = 2,
    UP_BOPDY = 3,
    LOW_BODY = 4
}
public class EnemyOnDamage : MonoBehaviour
{
    public BodyType bodyType = BodyType.NORMAL;
    EnemyControl parent;
    void Start()
    {
        parent = gameObject.GetComponentInParent<EnemyControl>();
    }
    public void OnDamageBullet(Bulletdata bulletdata)
    {
        bulletdata.rig_body = gameObject.GetComponent<Rigidbody>();
        bulletdata.bodyType = bodyType;
        parent.OnDamageBullet(bulletdata);
    }
    public void OnDamageGrenade(Grenadedata grenadedata)
    {
        parent.OnDamageGrenade(grenadedata);
    }
}

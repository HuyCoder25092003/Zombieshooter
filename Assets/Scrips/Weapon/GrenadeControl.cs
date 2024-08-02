using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grenadedata
{
    public int damage;
    public int radius;
}
public class GrenadeControl : MonoBehaviour
{
    Transform trans;
    Grenadedata data;
    public LayerMask mask;
    private void Awake()
    {
        trans = transform;
    }
    IEnumerator OnEndLife()
    {
        yield return new WaitForSeconds(3);
        BYPoolManager.instance.GetPool("Grenade").Despawn(transform);
    }
    public void OnSpawn()
    {
        StopCoroutine("OnEndLife");
        StartCoroutine("OnEndLife");
    }
    // Update is called once per frame
    void Update()
    {
        Transform effect = BYPoolManager.instance.GetPool("BigExplosion").Spawn();
        effect.position = transform.position;
        effect.rotation = transform.rotation;
        Collider[] colliders = Physics.OverlapSphere(transform.position, data.radius,mask);
        foreach (Collider nearByObject in colliders)
        {
            EnemyOnDamage enemy = nearByObject.GetComponent<EnemyOnDamage>();
            enemy?.OnDamageGrenade(data);
        }

    }
    public void Setup(Grenadedata data)
    {
        this.data = data;
    }
}

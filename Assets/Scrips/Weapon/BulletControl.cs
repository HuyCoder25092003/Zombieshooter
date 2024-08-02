using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bulletdata
{
    public string name_pool;
    public int damage = 0;
    public Vector3 force;
    public Vector3 point_impact;
    public Rigidbody rig_body;
    public BodyType bodyType;
}
public class BulletControl : MonoBehaviour
{
    Transform trans;
    Bulletdata data;
    public LayerMask mask;
    void Awake()
    {
        trans = transform;
    }
    IEnumerator OnEndLife()
    {
        yield return new WaitForSeconds(3);
        BYPoolManager.instance.dic_pool[data.name_pool].Despawn(transform);
    }
    public void OnSpawn()
    {
        StopCoroutine("OnEndLife");
        StartCoroutine("OnEndLife");
    }
    void Update()
    {
        trans.Translate(Vector3.forward * Time.deltaTime * 20);
        if (Physics.Raycast(trans.position, trans.forward, out RaycastHit hitInfo, 1, mask))
        {
            Transform impact = BYPoolManager.instance.dic_pool["Impact"].Spawn();
            impact.position = hitInfo.point;
            impact.forward = hitInfo.normal;

            BYPoolManager.instance.dic_pool[data.name_pool].Despawn(trans);

            data.point_impact = hitInfo.point;
            EnemyOnDamage enemyOnDamage = hitInfo.collider.GetComponent<EnemyOnDamage>();
            if (enemyOnDamage != null)
                enemyOnDamage.OnDamageBullet(data);
        }
    }
    public void Setup(Bulletdata data)
    {
        this.data = data;
    }
    private void OnBecameInvisible()
    {
        BYPoolManager.instance.dic_pool[data.name_pool].Despawn(transform);
    }
}

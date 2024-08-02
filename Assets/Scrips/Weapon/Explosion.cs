using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    IEnumerator OnEndLife()
    {
        yield return new WaitForSeconds(1);
        BYPoolManager.instance.dic_pool["BigExplosion"].Despawn(transform);
    }
    public void OnSpawn()
    {
        StopCoroutine("OnEndLife");
        StartCoroutine("OnEndLife");
    }
}

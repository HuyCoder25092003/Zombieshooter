using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactControl : MonoBehaviour
{
    IEnumerator OnEndLife()
    {
        yield return new WaitForSeconds(1);
        BYPoolManager.instance.dic_pool["Impact"].Despawn(transform);
    }
    public void OnSpawn()
    {
        StopCoroutine("OnEndLife");
        StartCoroutine("OnEndLife");
    }
}

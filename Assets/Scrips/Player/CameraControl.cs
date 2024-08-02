using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target_trans;
    private void LateUpdate()
    {
        transform.position = target_trans.position;
    }

}

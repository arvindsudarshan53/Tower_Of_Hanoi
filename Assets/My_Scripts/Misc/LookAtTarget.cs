using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField]
    Transform targetObj;

    private void Start()
    {
        if (targetObj == null)
            targetObj = Camera.main.transform;
            
    }

    void Update()
    {
            transform.LookAt(targetObj);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscProps : MonoBehaviour
{

    public int discSize;

    public Vector3 initPos;

    private void Awake()
    {
        GetInitPos();
    }

    void GetInitPos()
    {
        initPos = transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiscProps : MonoBehaviour
{

    public int discSize;
    public TextMeshProUGUI sizeDispText;

    [HideInInspector] public Vector3 initPos;

    private void Awake()
    {
        GetInitPos();
    }

    void GetInitPos()
    {
        initPos = transform.position;
    }
}

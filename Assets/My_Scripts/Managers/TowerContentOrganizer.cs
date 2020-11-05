using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerContentOrganizer : MonoBehaviour
{

    public List<Transform> thisTowerDiscs;

    public void AddThisDisc(Transform thisDisc)
    {
        thisTowerDiscs.Insert(0,thisDisc);
    }

    public void RemoveThisDisc(Transform thisDisc)
    {
        thisTowerDiscs.Remove(thisDisc);
    }
}

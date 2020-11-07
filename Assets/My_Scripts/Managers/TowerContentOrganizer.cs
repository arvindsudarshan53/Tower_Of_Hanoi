using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerContentOrganizer : MonoBehaviour // For storing and maintaining contents of Tower
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

    public Transform GetTopDisc()
    {
        if (thisTowerDiscs.Count > 0)
            return thisTowerDiscs[0];
        else
            return null;
    }

    public int GetDiscSize(Transform thisDisc)
    {
        return thisDisc.GetComponent<DiscProps>().discSize;
    }

    public void ClearTempData()
    {
        thisTowerDiscs.Clear();
    }
}

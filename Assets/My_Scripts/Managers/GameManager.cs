using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    GameObject[] discGameObjs;

    [SerializeField]
    int chosenNoOfDiscs = 7;

    public TowerContentOrganizer towerAContentOrganizer, towerBContentOrganizer, towerCContentOrganizer;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ClampChosenNoOfDiscs();
        SetupDiscs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupDiscs()
    {
        for(int currentIndex = 0; currentIndex < discGameObjs.Length; currentIndex++)
        {
            if(currentIndex < chosenNoOfDiscs)
            {
                discGameObjs[currentIndex].SetActive(true);
                towerAContentOrganizer.AddThisDisc(discGameObjs[currentIndex].transform);

            }
            else
            {
                discGameObjs[currentIndex].SetActive(false);
            }
        }
    }

    void ClampChosenNoOfDiscs()
    {
        chosenNoOfDiscs = Mathf.Clamp(chosenNoOfDiscs, 1, 7);
    }
}

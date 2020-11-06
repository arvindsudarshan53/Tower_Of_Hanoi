using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    GameObject[] discGameObjs;

    UIManager uiManager;
    UndoManager undoManager;

    [HideInInspector] public int chosenNoOfDiscs = 7;
    [HideInInspector] public bool isPlaying = false;
    [HideInInspector] public int noOfMoves = 0;

    public TowerContentOrganizer towerAContentOrganizer, towerBContentOrganizer, towerCContentOrganizer;



    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
        undoManager = UndoManager.instance;

        uiManager.ShowSetupMenu(true);
        uiManager.ShowInGameUI(false);
    }


    public void StartTheGame()
    {
        ClampChosenNoOfDiscs();
        SetupDiscs();
        uiManager.ShowSetupMenu(false);
        uiManager.ShowInGameUI(true);
        noOfMoves = 0;
        undoManager.ClearAllUndoMoves();
        isPlaying = true;
    }

    public void RestartTheGame()
    {
        ClearAllProgress();
        SetInitPositionsOfDiscs();
        SetupDiscs();
        uiManager.ShowDiscIndicator(false,"");
        undoManager.ClearAllUndoMoves();
        noOfMoves = 0;
    }

    public void BackToMenu()
    {
        uiManager.ShowSetupMenu(true);
        uiManager.ShowInGameUI(false);
        ClearAllProgress();
        SetInitPositionsOfDiscs();
        isPlaying = false;
        uiManager.ShowDiscIndicator(false, "");
    }

    void ClearAllProgress()
    {
        towerAContentOrganizer.ClearTempData();
        towerBContentOrganizer.ClearTempData();
        towerCContentOrganizer.ClearTempData();
        DiscMover.instance.ClearTempData();
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

    void SetInitPositionsOfDiscs()
    {
        foreach(GameObject g in discGameObjs)
        {
            g.transform.position = g.GetComponent<DiscProps>().initPos;
            g.GetComponent<Rigidbody>().isKinematic = false;
            g.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}

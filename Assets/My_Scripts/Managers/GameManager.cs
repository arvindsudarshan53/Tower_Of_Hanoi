using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour // Handles Game setup
{
    public static GameManager instance;

    [HideInInspector] public int chosenNoOfDiscs = 7;
    [HideInInspector] public bool isPlaying = false;
    [HideInInspector] public int noOfMoves = 0;
    public int bestMovesCount;
    public bool isAutoModeOn = false;

    [SerializeField] GameObject[] discGameObjs;

    UIManager uiManager;
    UndoManager undoManager;
    AutoModeManager autoModeManager;


    public TowerContentOrganizer towerAContentOrganizer, towerBContentOrganizer, towerCContentOrganizer;



    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        uiManager       = UIManager.instance;
        undoManager     = UndoManager.instance;
        autoModeManager = AutoModeManager.instance;
        uiManager.ShowSetupMenu(true);
        uiManager.ShowInGameUI(false);
    }


    public void StartTheGame()
    {
        ClearAllProgress();
        SetInitPositionsOfDiscs();
        ClampChosenNoOfDiscs();
        SetupDiscs();
        uiManager.ShowSetupMenu(false);
        uiManager.ShowInGameUI(true);
        noOfMoves = 0;
        undoManager.ClearAllUndoMoves();
        isPlaying = true;
        CheckForAutoModeAndExecute();
        GetBestMoveCount();
    }

    public void RestartTheGame()
    {
        ClearAllProgress();
        SetInitPositionsOfDiscs();
        SetupDiscs();
        uiManager.ShowDiscIndicator(false,"");
        undoManager.ClearAllUndoMoves();
        noOfMoves = 0;
        isPlaying = true;
        CheckForAutoModeAndExecute();
    }

    public bool CheckForWin() // When all discs are moved to Tower C
    {
        if (towerCContentOrganizer.thisTowerDiscs.Count == chosenNoOfDiscs)
            return true;
        else
            return false;
    }

    void CheckForAutoModeAndExecute()
    {
        if (isAutoModeOn)
        {
            autoModeManager.ClearAllAutoModeData();
            autoModeManager.SolvePuzzle(); // Gets the steps to solve the puzzle
            autoModeManager.FetchAutoModeMove(); // Fetches first AutoMode move
            uiManager.ShowUndoButton(false);
        }
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

    void ClearAllProgress() // General data clearance to delete all temp variables stored
    {
        towerAContentOrganizer.ClearTempData();
        towerBContentOrganizer.ClearTempData();
        towerCContentOrganizer.ClearTempData();
        DiscMover.instance.ClearTempData();
    }

    void SetupDiscs() // Shows only chosen number of discs
    {
        for(int currentIndex = 0; currentIndex < discGameObjs.Length; currentIndex++)
        {
            if(currentIndex < chosenNoOfDiscs)
            {
                discGameObjs[currentIndex].SetActive(true);
                discGameObjs[currentIndex].GetComponent<DiscProps>().sizeDispText.text = (chosenNoOfDiscs-(currentIndex)).ToString(); // to display size
                towerAContentOrganizer.AddThisDisc(discGameObjs[currentIndex].transform); // Adds the disc as a content of Tower A

            }
            else
            {
                discGameObjs[currentIndex].SetActive(false);
            }
        }
    }

    void ClampChosenNoOfDiscs() // To maintain No Of Disc 1 << N << 7
    {
        chosenNoOfDiscs = Mathf.Clamp(chosenNoOfDiscs, 1, 7);
    }

    void SetInitPositionsOfDiscs() // To Reset All Discs at initial position on Restart/Start
    {
        foreach(GameObject g in discGameObjs)
        {
            g.transform.position = g.GetComponent<DiscProps>().initPos;
            g.GetComponent<Rigidbody>().isKinematic = false;
            g.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    void GetBestMoveCount()
    {
        bestMovesCount = (int)(Mathf.Pow(2, chosenNoOfDiscs)) - 1;  // Formula 2 to the power of N (No of discs) - 1
    }
}

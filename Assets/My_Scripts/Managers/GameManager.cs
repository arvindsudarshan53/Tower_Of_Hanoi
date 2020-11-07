using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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

    public bool CheckForWin()
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
                discGameObjs[currentIndex].GetComponent<DiscProps>().sizeDispText.text = (chosenNoOfDiscs-(currentIndex)).ToString(); // to display size
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

    void GetBestMoveCount()
    {
        bestMovesCount = (int)(Mathf.Pow(2, chosenNoOfDiscs)) - 1;
    }
}

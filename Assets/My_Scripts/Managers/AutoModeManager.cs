using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoModeManager : MonoBehaviour // Handles Auto Mode
{

    public static AutoModeManager instance;

    public List<string> autoSolveSteps;

    public int         currentAutoMoveIndex = 0;
    GameManager gameManager;
    DiscMover   discMover;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        discMover = DiscMover.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SolvePuzzle()
    {
        autoSolveSteps.Clear();
        string startTower = "Tower_A"; // start tower in output
        string endTower = "Tower_C"; // end tower in output
        string tempTower = "Tower_B"; // temporary tower in output
        

        solveTowers(gameManager.chosenNoOfDiscs, startTower, endTower, tempTower);
    }

    private void solveTowers(int n, string startTower, string endTower, string tempTower)
    {
        if (n > 0)
        {
            solveTowers(n - 1, startTower, tempTower, endTower);
            autoSolveSteps.Add(startTower + "," + endTower);
            solveTowers(n - 1, tempTower, endTower, startTower);

        }
    }

    public void FetchAutoModeMove()
    {
        if (autoSolveSteps.Count <= 0)
            return;

        discMover.ExecuteThisAutoModeMove(autoSolveSteps[currentAutoMoveIndex]);
        currentAutoMoveIndex++;
    }

    public void ClearAllAutoModeData()
    {
        autoSolveSteps.Clear();
        currentAutoMoveIndex = 0;
    }
}

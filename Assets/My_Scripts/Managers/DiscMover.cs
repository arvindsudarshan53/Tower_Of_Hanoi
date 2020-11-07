using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscMover : MonoBehaviour // Handles Disc movement
{
    public static DiscMover instance;

    [SerializeField]
    Transform[] towerPositions;

    [SerializeField]
    float moveSpeed = 20f;

    bool canMove = false;

    public Transform discToMove;

    Rigidbody chosenDiscRigidBody;
    [SerializeField]
    string moveFromTowerIndex, moveToTowerIndex;

    int[] moveTargetsTowerIndex = new int[2] { 0, 2 };
    GameManager     gameManager;
    UIManager       uiManager;
    UndoManager     undoManager;
    AutoModeManager autoModeManager;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        gameManager     = GameManager.instance;
        uiManager       = UIManager.instance;
        undoManager     = UndoManager.instance;
        autoModeManager = AutoModeManager.instance;

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isPlaying)
            return;
        if(canMove && discToMove != null)
        {
            SetMoveTargetsTowerIndex();
            MoveDisc(discToMove);
        }
    }

    private void FixedUpdate()
    {
        if (!gameManager.isPlaying || gameManager.isAutoModeOn)
            return;
        PlayerInput();
    }

    int obtainedDiscSize;
    void PlayerInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canMove)
                return;
            int layer_mask = LayerMask.GetMask("Rod");
            int distance = 25;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance, layer_mask))
            {
                
                string chosenTowerName = hit.collider.transform.root.name; // Gets the Tower selection
                Transform obtainedDiscToMove = GetTopDiscFromTower(chosenTowerName);
                
                //print(hit.collider.name);
                if (moveFromTowerIndex == "" && obtainedDiscToMove != null) // First Tower Selection
                {
                    uiManager.ShowDiscIndicator(true, chosenTowerName);
                    obtainedDiscSize = GetDiscSize(chosenTowerName, obtainedDiscToMove); // Gets the size of chosen disc to move
                    moveFromTowerIndex = chosenTowerName;
                    discToMove = obtainedDiscToMove;
                    chosenDiscRigidBody = discToMove.GetComponent<Rigidbody>();
                }

                if(moveFromTowerIndex != ""
                    && moveFromTowerIndex != chosenTowerName) // Second Tower Selection
                {
                    Transform topDiscFromDestTower = GetTopDiscFromTower(chosenTowerName);
                    if(topDiscFromDestTower == null) // When Destination Tower is Empty
                    {
                        uiManager.ShowDiscIndicator(false,"");
                        moveToTowerIndex = chosenTowerName;
                        canMove = true;
                        chosenDiscRigidBody.isKinematic = true;
                        chosenDiscRigidBody.useGravity = false;
                        OrganizeTowers();
                        return;
                    }
                    else if (obtainedDiscSize < GetDiscSize(chosenTowerName, topDiscFromDestTower)) // When Destination tower has discs and compares sizes of discs
                    {
                        uiManager.ShowDiscIndicator(false,"");
                        moveToTowerIndex = chosenTowerName;
                        canMove = true;
                        chosenDiscRigidBody.isKinematic = true;
                        chosenDiscRigidBody.useGravity = false;
                        OrganizeTowers();
                        return;
                    }
                    else if(obtainedDiscSize > GetDiscSize(chosenTowerName, topDiscFromDestTower)) // When second tower chosen contains bigger disc size prompt "wrong move"
                    {
                        moveFromTowerIndex = "";
                        uiManager.ShowDiscIndicator(false,"");
                        uiManager.ShowWrongMoveText();
                        return;
                    }
                }
            }
        }
    }

    int index = 0;
   
    private void MoveDisc(Transform thisDisc) // Handles Disc movement from tower to tower
    {

        thisDisc.position = Vector3.MoveTowards(thisDisc.position,
            towerPositions[moveTargetsTowerIndex[index]].position,
            moveSpeed);

        if (Vector3.Distance(towerPositions[moveTargetsTowerIndex[index]].position, thisDisc.position) <= 0)
        {
            if (index == 1)
            {
                index = 0;
                canMove = false;
                chosenDiscRigidBody = discToMove.GetComponent<Rigidbody>();
                chosenDiscRigidBody.isKinematic = false;
                chosenDiscRigidBody.useGravity = true;
                if(!undoManager.isUndoing)
                {
                    string movesEncodeToStr = discToMove.name +","+moveFromTowerIndex + "," + moveToTowerIndex;
                    undoManager.AddThisMove(movesEncodeToStr); // Adds the player's move
                    gameManager.noOfMoves++;
                }
                else // When Undo is done add discs to respective towers
                {
                    OrganizeTowers(); 
                }
                if(gameManager.isAutoModeOn)
                {
                    OrganizeTowers();
                    Invoke("GetNextAutoMove",1);
                }
                moveFromTowerIndex = "";
                moveToTowerIndex = "";
                undoManager.isUndoing = false;
                if(undoManager.listOfMoves.Count > 0 && !gameManager.isAutoModeOn)
                    uiManager.ShowUndoButton(true);

                if(gameManager.CheckForWin())
                {
                    if (gameManager.isAutoModeOn)
                        uiManager.ShowSolvedText();
                    else
                        uiManager.ShowYouWonText();
                    gameManager.isPlaying = false;
                }
            }
            else
            {
                index++;
            }
        }
    }

    void GetNextAutoMove() // On Auto solve mode this will fetch next move
    {
        if (autoModeManager.currentAutoMoveIndex < autoModeManager.autoSolveSteps.Count)
            autoModeManager.FetchAutoModeMove(); // Fetches Next AutoMode move
    }

    private void SetMoveTargetsTowerIndex() 
    {
        if (moveFromTowerIndex == "Tower_A")
            moveTargetsTowerIndex[0] = 0;
        else if (moveFromTowerIndex == "Tower_B")
            moveTargetsTowerIndex[0] = 1;
        else
            moveTargetsTowerIndex[0] = 2;

        if (moveToTowerIndex == "Tower_A")
            moveTargetsTowerIndex[1] = 0;
        else if (moveToTowerIndex == "Tower_B")
            moveTargetsTowerIndex[1] = 1;
        else
            moveTargetsTowerIndex[1] = 2;
    }

    void OrganizeTowers()// Handles the content of towers
    {
        if (moveFromTowerIndex == "Tower_A")
            gameManager.towerAContentOrganizer.RemoveThisDisc(discToMove);
        else if (moveFromTowerIndex == "Tower_B")
            gameManager.towerBContentOrganizer.RemoveThisDisc(discToMove);
        else
            gameManager.towerCContentOrganizer.RemoveThisDisc(discToMove);

        if (moveToTowerIndex == "Tower_A")
            gameManager.towerAContentOrganizer.AddThisDisc(discToMove);
        else if (moveToTowerIndex == "Tower_B")
            gameManager.towerBContentOrganizer.AddThisDisc(discToMove);
        else
            gameManager.towerCContentOrganizer.AddThisDisc(discToMove);
    }

    Transform GetTopDiscFromTower(string towerName)
    {
        if (towerName == "Tower_A")
            return gameManager.towerAContentOrganizer.GetTopDisc();
        else if (towerName == "Tower_B")
            return gameManager.towerBContentOrganizer.GetTopDisc();
        else if (towerName == "Tower_C")
            return gameManager.towerCContentOrganizer.GetTopDisc();

        return null;
    }

    int GetDiscSize(string towerName, Transform thisDisc)
    {
        if (towerName == "Tower_A")
            return gameManager.towerAContentOrganizer.GetDiscSize(thisDisc);
        else if (towerName == "Tower_B")
            return gameManager.towerBContentOrganizer.GetDiscSize(thisDisc);
        else if (towerName == "Tower_C")
            return gameManager.towerCContentOrganizer.GetDiscSize(thisDisc);
        return 0;
    }

    public void ClearTempData() // To clear temp data of this class
    {
        moveFromTowerIndex = "";
        moveToTowerIndex = "";
        canMove = false;
    }


    public void UndoTheMove(string undoEncodedMove) // When Undo button is clicked this handles the undo action
    {
        string[] decodedUndoMove = undoEncodedMove.Split(',');

        moveFromTowerIndex = decodedUndoMove[2];
        moveToTowerIndex = decodedUndoMove[1];
        discToMove = GameObject.Find(decodedUndoMove[0]).transform;
        Rigidbody discToMoveRigidBody = discToMove.GetComponent<Rigidbody>();
        discToMoveRigidBody.isKinematic = true;
        discToMoveRigidBody.useGravity = false;
        canMove = true;
    }

    public void ExecuteThisAutoModeMove(string obtainedAutoModeMove) // this handles the auto moves
    {
        string[] decodedMove = obtainedAutoModeMove.Split(',');
        moveFromTowerIndex = decodedMove[0];
        moveToTowerIndex = decodedMove[1];
        discToMove = GetTopDiscFromTower(moveFromTowerIndex);
        if(discToMove != null)
        {
            Rigidbody discToMoveRigidBody = discToMove.GetComponent<Rigidbody>();
            discToMoveRigidBody.isKinematic = true;
            discToMoveRigidBody.useGravity = false;

        }
        canMove = true;
    }

}

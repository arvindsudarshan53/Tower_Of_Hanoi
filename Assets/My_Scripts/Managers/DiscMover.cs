using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscMover : MonoBehaviour
{

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
    GameManager gameManager;
    UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        
        gameManager = GameManager.instance;
        uiManager = UIManager.instance;
        //chosenDiscRigidBody.isKinematic = true;
        //chosenDiscRigidBody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            //moveFromTowerIndex = "Tower_A";
            //moveToTowerIndex = "Tower_C";
            SetMoveTargetsTowerIndex();
            MoveDisc(discToMove);
        }
    }

    private void FixedUpdate()
    {
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
                if (moveFromTowerIndex == "" && obtainedDiscToMove != null)
                {
                    uiManager.ShowDiscIndicator(true, chosenTowerName);
                    obtainedDiscSize = GetDiscSize(chosenTowerName, obtainedDiscToMove); // Gets the size of chosen disc to move
                    moveFromTowerIndex = chosenTowerName;
                    discToMove = obtainedDiscToMove;
                    chosenDiscRigidBody = discToMove.GetComponent<Rigidbody>();
                }

                if(moveFromTowerIndex != ""
                    && moveFromTowerIndex != chosenTowerName)
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
                    else if(obtainedDiscSize > GetDiscSize(chosenTowerName, topDiscFromDestTower))
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
   
    private void MoveDisc(Transform thisDisc)
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
                chosenDiscRigidBody.isKinematic = false;
                chosenDiscRigidBody.useGravity = true;
                moveFromTowerIndex = "";
                moveToTowerIndex = "";
            }
            else
            {
                index++;
            }
        }
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

    void OrganizeTowers()
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

}

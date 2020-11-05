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
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
        gameManager = GameManager.instance;
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
                
                string hitObjName = hit.collider.transform.root.name;
                Transform obtainedDiscToMove = GetTopDiscFromTower(hitObjName);
                //print(hit.collider.name);
                if (moveFromTowerIndex == "" && obtainedDiscToMove != null)
                {
                    moveFromTowerIndex = hitObjName;
                    discToMove = obtainedDiscToMove;
                    chosenDiscRigidBody = discToMove.GetComponent<Rigidbody>();
                }

                if(moveFromTowerIndex != "" && moveFromTowerIndex != hitObjName)
                {
                    moveToTowerIndex = hitObjName;
                    canMove = true;
                    chosenDiscRigidBody.isKinematic = true;
                    chosenDiscRigidBody.useGravity = false;
                    OrganizeTowers();
                }
            }
        }
    }

    int index = 0;
    float distBtwDiscAndTarget;
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
        if (towerName == "Tower_A" && gameManager.towerAContentOrganizer.thisTowerDiscs.Count > 0)
            return gameManager.towerAContentOrganizer.thisTowerDiscs[0];
        else if (towerName == "Tower_B" && gameManager.towerBContentOrganizer.thisTowerDiscs.Count > 0)
            return gameManager.towerBContentOrganizer.thisTowerDiscs[0];
        else if (towerName == "Tower_C" && gameManager.towerCContentOrganizer.thisTowerDiscs.Count > 0)
            return gameManager.towerCContentOrganizer.thisTowerDiscs[0];

        return null;
    }

}

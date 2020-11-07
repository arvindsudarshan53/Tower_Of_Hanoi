using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoManager : MonoBehaviour // Handles Undo functionality
{
    public static UndoManager instance;

    public List<string> listOfMoves;
    public bool isUndoing = false;

    private void Awake()
    {
        instance = this;
    }

    public void AddThisMove(string currentMove)
    {
        listOfMoves.Add(currentMove);
    }

    public void ClearAllUndoMoves()
    {
        listOfMoves.Clear();
    }

    public void UndoLastMove()
    {
        if (listOfMoves.Count > 0)
        {
            UIManager.instance.ShowUndoButton(false);
            isUndoing = true;
            string lastMoveStr = listOfMoves[listOfMoves.Count - 1];
            DiscMover.instance.UndoTheMove(lastMoveStr);
            listOfMoves.RemoveAt(listOfMoves.Count-1);
        }

    }
}

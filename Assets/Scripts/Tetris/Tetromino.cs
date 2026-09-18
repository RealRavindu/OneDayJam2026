using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
//This class is an interface to move the Tetromino down
//This class builds the tetromino based on the tetrominodata
public class Tetromino : MonoBehaviour
{
    public TetrominoShape shape;
    public int rotationIndex
    {
        get { return _rotationIndex; }
        set
        {
            if (value > 3) value = 0;
            if (value < 0) value = 3;
            _rotationIndex = value;
        }
    }
    private int _rotationIndex; //used by the Super Rotation System (SRS) to get the correct test sequence. The SRS script has a more detailed explanation.
    public Block[] blocksList = new Block[4];
    public LayerMask LM_Tetromino;
    public Dictionary<int, List<Vector2Int>> testSequence;
    public void InstantiateTetromino(TetrominoData data)
    {
        shape = data.shape;
        testSequence = (shape == TetrominoShape.I) ? SRS.instance.I_TestSequences : SRS.instance.O_TestSequences;

        for (int i = 0; i < 4; i++)
        {
            blocksList[i].InstantiateBlock(data.blockPositions[i], this);
        }
    }
    public void MoveTetrominoTo(Vector2 position)
    {

        transform.position = position;

    }

    public bool CanTetrominoMoveTo(Vector2 position)
    {
        bool canMove = true;
        foreach (Block block in blocksList)
        {
            if (IsThereColliderAtPosition(position + block.position))
            {
                canMove = false;
                break;
            }
        }

        return canMove;
    }
    public void RotateTetromino(Vector2 rotationMatrix)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2 currentPos = blocksList[i].position;
            blocksList[i].position = new Vector2(currentPos.y, currentPos.x) * rotationMatrix;
        }
        rotationIndex++;
    }

    public bool CanTetrominoRotateTo(Vector2[] positionList, Vector2 offset)
    {
        bool canMove = true;
        foreach (Vector2 pos in positionList)
        {
            if (IsThereColliderAtPosition(offset + pos + (Vector2)transform.position))
            {
                canMove = false;
                break;
            }
        }
        return canMove;
    }

    public void SlamTetromino()
    {
        MoveTetrominoTo(GetLowestPositionTetrominoCanMoveTo());
        GameManager.instance.Tick(); //force a tick after slamming
    }

    public bool IsThereColliderAtPosition(Vector2 targPosition)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(targPosition, transform.localScale / 2, 0, Vector2.down, 0, LM_Tetromino);
        foreach (RaycastHit2D hit in hits)
        {
            if (!blocksList.Contains<Block>(hit.collider.GetComponent<Block>())) { return true; }
        }
        return false;
    }
    //for shadow and slam calculator
    public Vector2 GetLowestPositionTetrominoCanMoveTo()
    {
        Debug.Log("\n");
        float highestY = -Mathf.Infinity;
        int lowestBlockNum = 0;
        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(blocksList[i].transform.position + Vector3.down, Vector2.down, 40, LM_Tetromino);
            float hitY = Mathf.Round(hit.point.y + 0.5f);
            Debug.Log($"{blocksList[i].name} with position.y {blocksList[i].position.y} has detected {hit.collider.name} at point {hit.point.y} rounded up to {hitY}");
            if (!blocksList.Contains<Block>(hit.collider.GetComponent<Block>()))
            {
                if (hitY >= highestY)
                {
                    //FOR FUTURE DEBUGGING TRY TO USE DISTANCE INSTEAD OF HIGHEST Y TO RESOLVE
                    Debug.Log("higher than or equal Y detected");
                    highestY = hitY;
                    lowestBlockNum = i;
                    Debug.Log($"y of block being currently checked {blocksList[i].position.y} y of lowest block {blocksList[lowestBlockNum].position.y}");
                    /*if (blocksList[i].position.y < blocksList[lowestBlockNum].position.y)
                    {
                        Debug.Log("block with lower position detected!");
                        lowestBlockNum = i;
                    }*/
                }
            }

            
        }
        Debug.Log("Selected highest Y: " + highestY);
        Debug.Log("Selected block: " + blocksList[lowestBlockNum].name);
        Transform blockTransform = blocksList[lowestBlockNum].transform;
        Debug.Log($"(transform.postion.y) {transform.position.y} - (block.position.y) {blockTransform.position.y}" +
            $" + (highestY) {highestY} + (blocklocalScale/2) {blockTransform.localScale.y/2} = (final vector){new Vector2(transform.position.x, transform.position.y - blockTransform.position.y + highestY + (blockTransform.localScale.y / 2))}");
        return new Vector2(transform.position.x, transform.position.y - blockTransform.position.y + highestY);
    }

}

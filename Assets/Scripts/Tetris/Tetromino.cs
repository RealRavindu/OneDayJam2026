using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
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
    private int _rotationIndex;
    public Block[] blocksList = new Block[4];
    public LayerMask LM_Tetromino;
    public void InstantiateTetromino(TetrominoData data)
    {
        shape = data.shape;
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
            if (block.IsThereColliderAt(position))
            {
                canMove = false;
                break;
            }
        }

        return canMove;
    }
    public void RotateTetrominoTo(Vector2[] positionsList, Vector2 offset)
    {
        for (int i = 0; i < 4; i++)
        {
            blocksList[i].position = positionsList[i];

        }
        transform.position += (Vector3)offset;
        rotationIndex++;

    }
    public bool CanTetrominoRotateTo(Vector2[] positionList, Vector2 offset)
    {
        bool canMove = true;
        foreach (Vector2 pos in positionList)
        {
            if (blocksList[0].IsThereColliderAt(offset + pos + (Vector2)transform.position))
            {
                canMove = false;
                break;
            }
        }
        return canMove;
    }

    public void SlamTetromino()
    {
        MoveTetrominoTo(GetHighestPointOfContact());
        GameManager.instance.Tick(); //force a tick after slamming
    }

    public Vector2 GetHighestPointOfContact()
    {
        float highestY = -Mathf.Infinity;
        int highestBlockNum = 0;
        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(blocksList[i].transform.position + Vector3.down, Vector2.down, 40, LM_Tetromino);
            if (!blocksList.Contains<Block>(hit.collider.GetComponent<Block>()))
            {
                if (hit.point.y > highestY)
                {
                    highestY = hit.point.y;
                    highestBlockNum = i;
                }
            }
        }
        Transform blockTransform = blocksList[highestBlockNum].transform;
        return new Vector2(transform.position.x, transform.position.y - blockTransform.position.y + highestY + (blockTransform.localScale.y / 2));
    }

}

using UnityEngine;
using System.Collections.Generic;
using static Unity.Collections.AllocatorManager;
public class TetrisController : MonoBehaviour
{
    public KeyCode rotateClockwise, rotateCounterClockwise, moveLeft, moveRight, boost;
    public KeyCode rotateClockwiseAlt, rotateCounterClockwiseAlt, moveLeftAlt, moveRightAlt, boostAlt;
    public float boostValue;
    [Header("Super Rotation System")]
    public Vector3Int[] I_SRSPositions;
    public Vector3Int[] Other_SRSPositions;
    private void Update()
    {
        if (GameManager.instance.activeTetris != null)
        {
            if (Input.GetKeyDown(rotateClockwise) || Input.GetKeyDown(rotateClockwiseAlt)) RotateClockwise();
            if (Input.GetKeyDown(rotateCounterClockwise) || Input.GetKeyDown(rotateCounterClockwiseAlt)) RotateCounterClockwise();
            if (Input.GetKeyDown(moveLeft) || Input.GetKeyDown(moveLeftAlt)) MoveLeft();
            if (Input.GetKeyDown(moveRight) || Input.GetKeyDown(moveRightAlt)) MoveRight();
            if (Input.GetKeyDown(boost) || Input.GetKeyDown(boostAlt)) Boost();
            if (Input.GetKeyUp(boost) || Input.GetKeyDown(boostAlt)) RemoveBoost();
        }
        
    }

    public void RotateClockwise()
    {
        bool canMove = true;
        Tetris tetris = GameManager.instance.activeTetris;

        //get center of all blocks
        Vector3 center = Vector3.zero;
        foreach(Block block in tetris.blocksList)
        {
            center += block.transform.position;
        }
        center /= 4;
        Vector3Int[] SRSPositions;
        if (tetris.shape == TetrisShape.I) SRSPositions = I_SRSPositions;
        else SRSPositions = Other_SRSPositions;
        Debug.Log($"///////////////ROTATION STARTED/////////////////// tetris position: {tetris.transform.position}");
        foreach (Vector3Int offset in SRSPositions)
        {
            Debug.Log($"offset: {offset}");
            foreach (Block block in tetris.blocksList)
            {
                //take a block and create a null object on top of it. Then rotate it.
                GameObject nullObject = new GameObject();
                nullObject.transform.position = block.transform.position;
                nullObject.transform.RotateAround(center, Vector3.forward, -90);
                Debug.Log($"null object transform position after rotation: {nullObject.transform.position}");
                nullObject.transform.position += offset;
                nullObject.transform.position = GameManager._tileMap.WorldToCell(nullObject.transform.position);
                Debug.Log($"null object transform position after offset: {nullObject.transform.position}");

                //boxcast checks if it will collide with anything in it's new position
                RaycastHit2D[] hits = Physics2D.BoxCastAll(block.transform.position, block.transform.localScale, 360, Vector2.up, 0, tetris.LM_Tetromino);
                foreach (RaycastHit2D hit in hits)
                {
                    if (!tetris.blocksList.Contains(hit.collider.GetComponent<Block>()))
                    {
                        //will only run if foreign object collides with null object
                        canMove = false;
                        Debug.Log($"Cannot rotate during offset {offset}, obstruction detected");
                    }
                }
                if (!canMove) break;
            }
            if (canMove)
            {
                Debug.Log($"Rotation is possible");
                //this will only run if an obstruction was NOT detected during the current offset
                tetris.transform.RotateAround(center, Vector3.forward, -90);
                tetris.transform.position += offset;
                tetris.ResetTetrominoPositionToMatchGrid();
                break;
            }
        }

        

    }
    public void RotateCounterClockwise()
    {
        
        GameManager.instance.activeTetris.transform.Rotate(new Vector3(0, 0, -90));
        GameManager.instance.activeTetris.ResetTetrominoPositionToMatchGrid();

    }
    public void MoveLeft()
    {
        Tetris tetris = GameManager.instance.activeTetris;
        bool canMove = true;
        foreach (Block block in tetris.blocksList)
        {
            RaycastHit2D[] hitsForThisBlockBottom = Physics2D.RaycastAll(block.transform.position + Vector3.down * block.transform.localScale.x / 2, Vector2.left, block.transform.localScale.x, tetris.LM_Tetromino);
            RaycastHit2D[] hitsForThisBlockTop = Physics2D.RaycastAll(block.transform.position + Vector3.up * block.transform.localScale.x / 2, Vector2.left, block.transform.localScale.x, tetris.LM_Tetromino);
            List<RaycastHit2D> hitsForThisBlock = new List<RaycastHit2D>();
            hitsForThisBlock.AddRange(hitsForThisBlockTop);
            hitsForThisBlock.AddRange(hitsForThisBlockBottom); foreach (RaycastHit2D hit in hitsForThisBlock)
            {
                if (!tetris.blocksList.Contains(hit.collider.GetComponent<Block>()))
                {
                    canMove = false; break;
                }
            }
        }
        if (canMove)
        {
            tetris.transform.position += Vector3.left;
        }
        
    }
    public void MoveRight()
    {
        Tetris tetris = GameManager.instance.activeTetris;
        bool canMove = true;
        foreach (Block block in tetris.blocksList)
        {
            RaycastHit2D[] hitsForThisBlockBottom = Physics2D.RaycastAll(block.transform.position + Vector3.down * block.transform.localScale.x / 2, Vector2.right, block.transform.localScale.x, tetris.LM_Tetromino);
            RaycastHit2D[] hitsForThisBlockTop = Physics2D.RaycastAll(block.transform.position + Vector3.up * block.transform.localScale.x / 2, Vector2.right, block.transform.localScale.x, tetris.LM_Tetromino);
            List<RaycastHit2D> hitsForThisBlock = new List<RaycastHit2D>();
            hitsForThisBlock.AddRange(hitsForThisBlockTop);
            hitsForThisBlock.AddRange(hitsForThisBlockBottom);
            foreach (RaycastHit2D hit in hitsForThisBlock)
            {
                if (!tetris.blocksList.Contains(hit.collider.GetComponent<Block>()))
                {
                    canMove = false; break;
                }
            }
        }
        if (canMove)
        {
            tetris.transform.position += Vector3.right;
        }

    }
    public void Boost()
    {
        GameManager.instance.tetrominoFallingSpeed += boostValue;
    }
    public void RemoveBoost()
    {
        GameManager.instance.tetrominoFallingSpeed -= boostValue;
    }
}

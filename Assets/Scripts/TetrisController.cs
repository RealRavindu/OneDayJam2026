using UnityEngine;
using System.Collections.Generic;
public class TetrisController : MonoBehaviour
{
    public KeyCode rotateClockwise, rotateCounterClockwise, moveLeft, moveRight, boost;
    public KeyCode rotateClockwiseAlt, rotateCounterClockwiseAlt, moveLeftAlt, moveRightAlt, boostAlt;
    public float boostValue;

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
        Vector3 center = GameManager.instance.activeTetris.blocksList[1].transform.position;
        GameManager.instance.activeTetris.transform.RotateAround(center, Vector3.forward, 90);
        GameManager.instance.activeTetris.ResetTetrominoPositionToMatchGrid();
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

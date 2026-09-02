using UnityEngine;

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
        //GameManager.instance.activeTetris.ResetTetrominoPositionToMatchGrid();
    }
    public void RotateCounterClockwise()
    {

        GameManager.instance.activeTetris.transform.Rotate(new Vector3(0, 0, -90));
        GameManager.instance.activeTetris.ResetTetrominoPositionToMatchGrid();
    }
    public void MoveLeft()
    {

        GameManager.instance.activeTetris.transform.position += Vector3.left;
        GameManager.instance.activeTetris.ResetTetrominoPositionToMatchGrid();
    }
    public void MoveRight()
    {

        GameManager.instance.activeTetris.transform.position += Vector3.right;
        GameManager.instance.activeTetris.ResetTetrominoPositionToMatchGrid();
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

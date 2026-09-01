using UnityEngine;

public class TetrisController : MonoBehaviour
{
    public KeyCode rotateClockwise, rotateCounterClockwise, moveLeft, moveRight, boost;
    public KeyCode rotateClockwiseAlt, rotateCounterClockwiseAlt, moveLeftAlt, moveRightAlt, boostAlt;
    public float boostValue;

    private void Update()
    {
        if (GamemANAGER.instance.activeTetris != null)
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

        GamemANAGER.instance.activeTetris.transform.Rotate(new Vector3(0, 0, 90));

    }
    public void RotateCounterClockwise()
    {

        GamemANAGER.instance.activeTetris.transform.Rotate(new Vector3(0, 0, -90));

    }
    public void MoveLeft()
    {

        GamemANAGER.instance.activeTetris.transform.position += Vector3.left;

    }
    public void MoveRight()
    {

        GamemANAGER.instance.activeTetris.transform.position += Vector3.right;

    }
    public void Boost()
    {
        GamemANAGER.instance.tetrominoFallingSpeed += boostValue;
    }
    public void RemoveBoost()
    {
        GamemANAGER.instance.tetrominoFallingSpeed -= boostValue;
    }
}

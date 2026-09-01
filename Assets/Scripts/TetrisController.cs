using UnityEngine;

public class TetrisController : MonoBehaviour
{
    public KeyCode rotateClockwise, rotateCounterClockwise, moveLeft, moveRight, boost;
    public float boostValue;

    private void Update()
    {
        if (Input.GetKeyDown(rotateClockwise)) RotateClockwise();
        if (Input.GetKeyDown(rotateCounterClockwise)) RotateCounterClockwise();
        if (Input.GetKeyDown(moveLeft)) MoveLeft();
        if (Input.GetKeyDown(moveRight)) MoveRight();
        if (Input.GetKeyDown(boost)) Boost();
        if (Input.GetKeyUp(boost)) RemoveBoost();
    }

    public void RotateClockwise()
    {
        foreach(Tetris tetromino in GamemANAGER.instance.ActivelyFallingTetrisList)
        {
            tetromino.transform.Rotate(new Vector3(0,0,90));
        }
    }
    public void RotateCounterClockwise()
    {

    }
    public void MoveLeft()
    {

    }
    public void MoveRight()
    {

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

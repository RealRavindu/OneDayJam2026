using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
//This class moves Tetrominos left and right
//This class rotates Tetrominos
public class TetrominoController : MonoBehaviour
{
    Tetromino activeTetromino => GameManager.instance.ActiveTetromino;
    [SerializeField] KeyCode leftKey, rightKey, clockWiseKey, slamKey; //not currently used
    [SerializeField] float inputBuffer, horizontalAfterBufferTime, verticalAfterBufferTime;
    private bool hasAlreadyMoved = false;

    [SerializeField] private Vector2 clockwiseRotationMatrix;
    private float time;

    private void Update()
    {
        //Horizontal movement
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            MoveKeyPressed(Vector2.right * Input.GetAxisRaw("Horizontal"), horizontalAfterBufferTime);
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            MoveKeyPressed(Vector2.down, verticalAfterBufferTime);
        }
        else hasAlreadyMoved = false;

        //Clockwise movement
        if (Input.GetKeyDown(clockWiseKey))
        {
            Dictionary<int, List<Vector2Int>> testSequence = (activeTetromino.shape == TetrominoShape.I) ? SRS.instance.I_TestSequences : SRS.instance.O_TestSequences;
            //get all positions to rotate tetromino to
            Vector2[] positionsToRotateTo = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                Vector2 currentPos = activeTetromino.blocksList[i].position;
                positionsToRotateTo[i] = new Vector2(currentPos.y, currentPos.x) * clockwiseRotationMatrix;
            }
            //try rotating tetromino to all offsets
            foreach (Vector2 offset in testSequence[activeTetromino.rotationIndex])
            {
                if (activeTetromino.CanTetrominoRotateTo(positionsToRotateTo, offset))
                {
                    activeTetromino.RotateTetrominoTo(positionsToRotateTo, offset);
                    break;
                }

            }

        }

        //Slam piece down
        if (Input.GetKeyDown(slamKey))
        {
            activeTetromino.SlamTetromino();
        }


    }

    void MoveKeyPressed(Vector2 direction, float bufferTime)
    {
        Vector2 position = direction + (Vector2)activeTetromino.transform.position;
        if (!hasAlreadyMoved)
        {
            time = 0;
            if (activeTetromino.CanTetrominoMoveTo(position))
            {
                activeTetromino.MoveTetrominoTo(position);
                hasAlreadyMoved = true;
            }

        }
        time += Time.deltaTime;
        if (time > inputBuffer)
        {
            if (activeTetromino.CanTetrominoMoveTo(position))
            {
                activeTetromino.MoveTetrominoTo(position);
            }

            time -= bufferTime;
        }
    }
}

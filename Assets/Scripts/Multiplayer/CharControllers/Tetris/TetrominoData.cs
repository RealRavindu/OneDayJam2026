using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
//This class contains data on the 7 different tetrominos i.e. where each block in a tetromino is placed.
[System.Serializable]
public class TetrominoData
{
    public TetrominoShape shape;
    public Vector2[] blockPositions = new Vector2[4];

}

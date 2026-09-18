using JetBrains.Annotations;
using UnityEngine;
using System.Linq;
//This class positions itself on instantiation
//This class checks for collisions in orthogonal directions
//This class moves in orthogonal directions
public class Block : MonoBehaviour
{
    public Tetromino tetromino;
    public Vector2 position
    {
        get { return _position; }
        set
        {
            //Vector2 offset = (tetromino.shape == TetrominoShape.I || tetromino.shape == TetrominoShape.O) ? new Vector2(0, 0.5f) : Vector2.zero;
            //Debug.Log($"AAAAAAAAAAAA {offset} original value: {value} thingamabobbed value {value + offset}  value added to transform { (Vector3)value + transform.parent.position} transform at {tetromino.transform.position}");
            
            transform.position = value + (Vector2)transform.parent.position;
            _position = value;
        }
    }
    [SerializeField]
    private Vector2 _position;
    public LayerMask LM_Tetromino;
    public void InstantiateBlock(Vector2 pos, Tetromino parentTetromino)
    {
        tetromino = parentTetromino;
        position = pos;
    }

    
}

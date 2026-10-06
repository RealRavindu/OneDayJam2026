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

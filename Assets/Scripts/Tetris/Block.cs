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
            Vector2 offset = (tetromino.shape == TetrominoShape.I || tetromino.shape == TetrominoShape.O) ? new Vector2(-0.5f, -0.5f) : Vector2.zero;
            value += offset;
            transform.position = (Vector3)value + transform.parent.position;
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

    public bool IsThereColliderAt(Vector2 targPosition)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(targPosition + (Vector2Int)TetrominoManager.tilemap.WorldToCell(position), transform.localScale / 2, 0, Vector2.down, 0, LM_Tetromino);
        foreach (RaycastHit2D hit in hits)
        {
            if (!tetromino.blocksList.Contains<Block>(hit.collider.GetComponent<Block>())) { return true; }
        }
        return false;
    }
}

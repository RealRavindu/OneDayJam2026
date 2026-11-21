using JetBrains.Annotations;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
//This class positions itself on instantiation
//This class checks for collisions in orthogonal directions
//This class moves in orthogonal directions
public class Block : NetworkBehaviour
{
    public Tetromino tetromino;
    public NetworkVariable<Vector2> position = new NetworkVariable<Vector2>(Vector2.zero);
    private void Start()
    {
        position.OnValueChanged += ChangePositionOfBlock;
    }
    private void ChangePositionOfBlock(Vector2 oldValue, Vector2 value)
    {
        Debug.Log("Block pos changed");
        transform.position = value + (Vector2)transform.parent.position;
    }
    public LayerMask LM_Tetromino;
    public void InstantiateBlock(Vector2 pos, Tetromino parentTetromino)
    {
        tetromino = parentTetromino;
        position.Value = pos;
    }
}

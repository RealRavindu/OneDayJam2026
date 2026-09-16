using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BlockShadow : MonoBehaviour
{
    public Block parentBlock;
    public Vector2 position
    {
        get
        {
            return _position;
        }
        set
        {
            //if (value.y == -Mathf.Infinity) Destroy(this);
            _position = (Vector3)TetrominoManager.tilemap.WorldToCell(value);
            transform.position = parentBlock.tetromino.GetHighestPointOfContact() + value;
        }
    }
    private Vector2 _position;

    public void Start()
    {
        parentBlock = transform.parent.GetComponent<Block>();
    }
    private void Update()
    {
        position = parentBlock.position;
        if (parentBlock.tetromino != GameManager.instance.ActiveTetromino) Destroy(this.gameObject);
    }

}




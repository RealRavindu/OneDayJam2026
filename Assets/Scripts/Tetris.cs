using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.LowLevelPhysics2D;

public class Tetris : MonoBehaviour
{
    public bool falling 
    {
        get { return _falling; }
        set { 
            _falling = value;
            if (!value) //if not falling
            {
                GamemANAGER.instance.activeTetris = null;
            }
        }
    }
    private bool _falling;
    public List<GameObject> blocksList = new List<GameObject>();
    public LayerMask tetrominoLayerMask;
    private void FixedUpdate()
    {
        if (falling)
        {
            transform.position = transform.position + Vector3.down * GamemANAGER.instance.tetrominoFallingSpeed * Time.deltaTime;

            CheckCollision();
        }
    }
    public void EnableFalling()
    {
        falling = true;
    }

    public void LandedOnTetromino()
    {
        falling = false;
        ResetTetrominoPositionToMatchGrid();

    }
    private void CheckCollision()
    {
        foreach (GameObject block in blocksList)
        {
            //raycast down(globally)
            Vector3 blockPos = block.transform.position;
            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(blockPos, Vector2.down, block.transform.localScale.x/2, tetrominoLayerMask); //only checking tetris collisions
            Debug.DrawLine(blockPos, blockPos + (Vector3)Vector2.down * block.transform.localScale.x/2);
            if(hits.Length>0)
            {
                foreach(RaycastHit2D hit in hits)
                {
                    if(hit.collider.gameObject != gameObject)
                    {
                        LandedOnTetromino();
                    }
                }
            }
            
        }
        
    }

    private void ResetTetrominoPositionToMatchGrid()
    {
        foreach(GameObject block in blocksList)
        {
            Vector3 blockPos = block.transform.position;
            block.transform.position = new Vector3(Mathf.Round(blockPos.x), Mathf.Round(blockPos.y), Mathf.Round(blockPos.z));
        }
    }
}

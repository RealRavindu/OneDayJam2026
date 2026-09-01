using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.LowLevelPhysics2D;
using Unity.VisualScripting;

public class Tetris : MonoBehaviour
{
    public bool falling 
    {
        get { return _falling; }
        set { 
            _falling = value;
            if (!value) //if not falling
            {
                Debug.Log($"I am a tetris piece and I have collided, my name is {gameObject.name}");
                GameManager.instance.activeTetris = null;
            }
        }
    }
    private bool _falling = false;
    public List<GameObject> blocksList = new List<GameObject>();
    public LayerMask tetrominoLayerMask;
    private void FixedUpdate()
    {
        if (falling)
        {
            transform.position = transform.position + Vector3.down * GameManager.instance.tetrominoFallingSpeed * Time.deltaTime;

            CheckCollision();
        }
    }
    public void EnableFalling()
    {
        falling = true;
    }

    public void LandedOnTetromino()
    {
        if (falling)
        {
            falling = false;
            ResetTetrominoPositionToMatchGrid();

            //check for Tetris
            List<int> yCoords = new List<int>();
            foreach (GameObject block in blocksList)
            {
                if (!yCoords.Contains((int)block.transform.position.y)) yCoords.Add((int)block.transform.position.y);
            }
            GridManager.instance.CheckForTetris(yCoords);


            //check if any block is out of bounds above camera
            foreach (GameObject block in blocksList)
            {
                float camHeightInWorld = Camera.main.ViewportToWorldPoint(Vector2.one).y;
                if (block.transform.position.y > camHeightInWorld) GameManager.instance.gameInPlay = false;
            }
        }
        
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
                    if(!blocksList.Contains(hit.collider.gameObject))
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

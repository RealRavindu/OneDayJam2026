using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.LowLevelPhysics2D;
using Unity.VisualScripting;

public class Tetris : MonoBehaviour
{
    public TetrisShape shape;
    public int rotationIndex
    {
        get { return _rotationIndex; }
        set
        {
            _rotationIndex = value;
            if (_rotationIndex > 3) _rotationIndex = 0;
        }
    }
    private int _rotationIndex;
    public bool falling 
    {
        get { return _falling; }
        set { 
            _falling = value;
            if (!value) //if not falling
            {
                foreach (Block block in blocksList) block.falling = false;
                Debug.Log($"I am a tetris piece and I have collided, my name is {gameObject.name}-elius tetrilius.");
                if(GameManager.instance.activeTetris == this) GameManager.instance.activeTetris = null;

            }
        }
    }
    private bool _falling = false;
    public List<Block> blocksList;
    public LayerMask LM_Tetromino;
    private void FixedUpdate()
    {
        if (falling)
        {
            CheckCollision();
        }
    }
    public void EnableFalling()
    {
        falling = true;
        foreach (Block block in blocksList) { block.falling = true; }
    }

    public void LandedOnTetromino()
    {
        if (falling)
        {
            falling = false;
            ResetTetrominoPositionToMatchGrid();

            //check for Tetris
            List<int> yCoords = new List<int>();
            foreach (Block block in blocksList)
            {
                block.partOfTetromino = false;
                int yCoord = (int)block.transform.position.y;

                if (!yCoords.Contains(yCoord))
                { 
                    yCoords.Add(yCoord);
                }
                
                GridManager.instance.AddBlockToDictionary(yCoord, block);
            }
            yCoords.Sort(); //lowest value number goes first
            GridManager.instance.CheckForTetris(yCoords);


            //check if any block is out of bounds above camera
            foreach (Block block in blocksList)
            {
                float camHeightInWorld = Camera.main.ViewportToWorldPoint(Vector2.one).y;
                if (block.transform.position.y > camHeightInWorld) GameManager.instance.gameInPlay = false;
            }
        }
        
    }
    private void CheckCollision()
    {
        foreach (Block block in blocksList)
        {
            //raycast down(globally)
            Vector3 blockPos = block.transform.position;
            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(blockPos, Vector2.down, block.transform.localScale.x/2, LM_Tetromino); //only checking tetris collisions
            Debug.DrawLine(blockPos, blockPos + (Vector3)Vector2.down * block.transform.localScale.x/2);
            if(hits.Length>0)
            {
                foreach(RaycastHit2D hit in hits)
                {
                    if(!blocksList.Contains(hit.collider.GetComponent<Block>()))
                    {
                        LandedOnTetromino();
                    }
                }
            }
            
        }
        
    }

    
    public void ResetTetrominoPositionToMatchGrid()
    {
        foreach(Block block in blocksList)
        {
            Vector3 blockPos = block.transform.position;
            block.transform.position = new Vector3(Mathf.Round(blockPos.x), blockPos.y, Mathf.Round(blockPos.z));
        }
    }
}

public enum TetrisShape
{
    I, O, J, L, T, Z, S
}

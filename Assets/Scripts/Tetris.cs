using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.LowLevelPhysics2D;

public class Tetris : MonoBehaviour
{
    public bool falling = false;
    public List<GameObject> blocksList = new List<GameObject>();
    public LayerMask tetrominoLayerMask;
    private void FixedUpdate()
    {
        if (falling)
        {
            CheckCollision();
            transform.position = transform.position + Vector3.down * GamemANAGER.instance.tetrominoFallingSpeed * Time.deltaTime;
        }
    }
    public void EnableFalling()
    {
        falling = true;
    }

    public void LandedOnTetromino()
    {
        falling = false;
    }
    private void CheckCollision()
    {
        foreach (GameObject block in blocksList)
        {
            //raycast down(globally)
            Vector3 blockPos = block.transform.position;
            RaycastHit2D hit;
            hit = Physics2D.Raycast(blockPos, Vector2.down, block.transform.localScale.x/2, tetrominoLayerMask);
            Debug.DrawLine(blockPos, blockPos + (Vector3)Vector2.down * block.transform.localScale.x/2);
            if(hit)
            {
                if (hit.collider.tag == this.tag)
                {
                    LandedOnTetromino();
                }
            }
            
        }
        
    }
}

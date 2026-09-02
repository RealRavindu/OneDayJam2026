using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public LayerMask LM_Tetromino;
    public Dictionary<int, List<Block>> placedBlocksList = new Dictionary<int, List<Block>>();
    public int Combo;
    private void Start()
    {
        instance = this;
    }

    public void CheckForTetris(List<int> YCoords)
    {
        foreach (int y in YCoords)
        {
            /* USING RAYCAST TO DETECT TETRIS
            Vector2 origin = new Vector2(-5, y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.right, 10, LM_Tetromino);
            if(hits.Length == 10)
            {
                Debug.Log($"TETRIS! in row {y}");
                Combo += 1;
                foreach (RaycastHit2D hit in hits) Destroy(hit.collider.gameObject); //destroy tetris line
                
            }
            */

            if (placedBlocksList[y].Count == 10)
            {
                Debug.Log($"TETRIS at {y}");
                foreach(Block block in placedBlocksList[y])
                {
                    Destroy(block.gameObject); //destroy the blocks from le game world
                    placedBlocksList[y] = new List<Block>(); //de list the blocks from le dictionary
                }

            } else
            {
                Debug.Log($"no tetris at {y}, no of blocks = {placedBlocksList[y].Count}");

            }

        }
    }

    public void AddBlockToDictionary(int y, Block block)
    {
        if (!placedBlocksList.ContainsKey(y))
        {
            placedBlocksList.Add(y, new List<Block>());
        }
        placedBlocksList[y].Add(block);
    }
}

using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public LayerMask LM_Tetromino;
    private void Start()
    {
        instance = this;
    }

    public void CheckForTetris(List<int> YCoords)
    {
        foreach (int y in YCoords)
        {
            Vector2 origin = new Vector2(-5, y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, Vector2.right, 10, LM_Tetromino);
            if(hits.Length == 10)
            {
                Debug.Log($"TETRIS! in row {y}");
            } else
            {
                Debug.Log($"no tetris, no. of blocks in row {y}: {hits.Length}");
            }
        }
    }
}

using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Block : MonoBehaviour
{
    public bool falling;
    public bool partOfTetromino = true;
    public LayerMask LM_Tetromino;

    private void Start()
    {
        LM_Tetromino = transform.parent.GetComponent<Tetris>().LM_Tetromino;
    }
    private void Update()
    {
        if (falling)
        {
            transform.position += Vector3.down * GameManager.instance.tetrominoFallingSpeed * Time.deltaTime;
            if (!partOfTetromino) CheckCollision();
        }
    }

    void CheckCollision()
    {
        //raycast down(globally)
        Vector3 blockPos = transform.position;
        RaycastHit2D[] hits;
        hits = Physics2D.RaycastAll(blockPos, Vector2.down, transform.localScale.x / 2, LM_Tetromino); //only checking tetris collisions
        Debug.DrawLine(blockPos, blockPos + (Vector3)Vector2.down * transform.localScale.x / 2);
        if (hits.Length > 0)
        {
            falling = false;
        }
    }

}

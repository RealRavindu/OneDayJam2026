using UnityEngine;
//This class keeps track of ticks (G steps).
//This class steps active Tetrominos down.
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Tetromino ActiveTetromino;
    [SerializeField] Vector2 spawnPosition;
    public float GStepTime;
    public float boostValue;
    private float currentTime;

    private void Start()
    {
        instance = this;
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        if( currentTime > GStepTime)
        {
            Tick();
        }
    }
    public void Tick()
    {
        StepActiveTetrominoDown();
        currentTime = 0;
    }
    void StepActiveTetrominoDown()
    {
        if (ActiveTetromino != null) 
        { 
            if(ActiveTetromino.CanTetrominoMoveTo(ActiveTetromino.transform.position + Vector3.down))
            {
                ActiveTetromino.MoveTetrominoTo(ActiveTetromino.transform.position + Vector3.down);
            } else
            {
                SpawnTetrominoAtTop();
            }
             
        }
    }

    public void SpawnTetrominoAtTop()
    {
        TetrominoManager.instance.SpawnRandomTetromino(spawnPosition);
    }

}

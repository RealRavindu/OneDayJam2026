using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GamemANAGER : MonoBehaviour 
{
    public static GamemANAGER instance;

    [Header("Gameplay variables")]
    public bool gameInPlay = true;
    public int score;
    public float timeToSpawnATetromino, tetrominoFallingSpeed;
    public List<Tetris> ActivelyFallingTetrisList = new List<Tetris>();
    [SerializeField] Vector3Int spawnPosition = new Vector3Int(-1, 8, 0); //probably dont change this or find a better way of doing this lul. dependant on camera positioning and allat.
    public float cameraMoveSpeed;
    private Tilemap _tileMap;
    [Header("Prefubs")]
    public List<GameObject> tetrominoPrefabsList = new List<GameObject>();
    private float currentTime;


    private GameObject TetriiGroup; //for organizing scene hierarchy while in game. Spawned Tetrominos will be parented under this empty gameobject

    
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TetriiGroup = new GameObject("TetriiGroup");
        _tileMap = FindAnyObjectByType<Tilemap>();
    }

    private void Update()
    {
        if(gameInPlay) currentTime += Time.deltaTime;

        MoveCameraUp();

        if ( currentTime > timeToSpawnATetromino)
        {
            currentTime = 0;
            SpawnTetrominoAtTop();
        }
    }

    public void SpawnTetrominoAtTop()
    {
        int randomNum = Random.Range(0, tetrominoPrefabsList.Count);
        GameObject spawnedTetromino = Instantiate(tetrominoPrefabsList[randomNum], TetriiGroup.transform);
        spawnedTetromino.transform.position = spawnPosition;
        Tetris tetrisScript = spawnedTetromino.GetComponent<Tetris>();
        ActivelyFallingTetrisList.Add(tetrisScript);
        tetrisScript.EnableFalling();
    }

    public void MoveCameraUp()
    {
        Camera.main.transform.position += Vector3.up * cameraMoveSpeed * Time.deltaTime;

        //TILE MAP BOX FILL OR SOMETHNIG HERE
    }


}

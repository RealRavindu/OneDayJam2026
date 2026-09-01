using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GamemANAGER : MonoBehaviour
{
    public static GamemANAGER instance;

    [Header("Gameplay variables")]
    public bool gameInPlay
    {
        get { return _gameInPlay; }
        set
        {
            _gameInPlay = value;
            if (value) SpawnTetrominoAtTop();
        }
    }
    private bool _gameInPlay = false;
    public int score;
    public float tetrominoFallingSpeed;
    public Tetris activeTetris
    {
        get { return _activeTetris; }
        set
        {
            _activeTetris = value;
            if (value == null && gameInPlay)
            {
                SpawnTetrominoAtTop();
            }
        }
    }
    private Tetris _activeTetris;
    [SerializeField] private int tetrominoSpawnOffset;
    public float cameraMoveSpeed;
    private Tilemap _tileMap;
    [SerializeField] private TileBase tile;
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
        gameInPlay = true;
    }

    private void Update()
    {
        if (gameInPlay) MoveCameraUp();

    }

    public void SpawnTetrominoAtTop()
    {
        int randomNum = Random.Range(0, tetrominoPrefabsList.Count);
        GameObject spawnedTetromino = Instantiate(tetrominoPrefabsList[randomNum], TetriiGroup.transform);
        Vector3Int spawnPosition = new Vector3Int(-1, (int)Camera.main.ViewportToWorldPoint(Vector2.one).y + tetrominoSpawnOffset);
        spawnedTetromino.transform.position = spawnPosition;
        activeTetris = spawnedTetromino.GetComponent<Tetris>();
        activeTetris.EnableFalling();
    }

    public void MoveCameraUp()
    {
        Camera.main.transform.position += Vector3.up * cameraMoveSpeed * Time.deltaTime;

        //adding tiles above camera
        Vector3 topLeftCorner = new Vector3(-4.5f, Camera.main.ViewportToWorldPoint(Vector2.one).y + 1);
        Vector3Int topLeftCornerINT = _tileMap.WorldToCell(topLeftCorner);
        for (int i = 0; i < 10; i++)
        {
            //setting individual tiles
            if (!_tileMap.HasTile(topLeftCornerINT))
            {
                _tileMap.SetTile(topLeftCornerINT, tile);
            }
            topLeftCornerINT.x += 1;
        }

        //remove tiles below camera
        Vector3 bottomLeftCorner = new Vector3(-4.5f, Camera.main.ViewportToWorldPoint(Vector2.zero).y - 1);
        Vector3Int bottomLeftCornerINT = _tileMap.WorldToCell(bottomLeftCorner);
        for (int i = 0; i < 10; i++)
        {
            if (_tileMap.HasTile(bottomLeftCornerINT))
            {
                _tileMap.SetTile(bottomLeftCornerINT, null);
            }
            bottomLeftCornerINT.x += 1;
        }
    }


}

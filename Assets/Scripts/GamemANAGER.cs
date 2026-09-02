using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Gameplay variables")]
    public bool gameInPlay
    {
        get { return _gameInPlay; }
        set
        {
            _gameInPlay = value; //when game starts
            if (value)
            {
                Debug.Log("Game started");
                SpawnTetrominoAtTop(GenerateRandomTetromino());
                SelectThreeTetrominoToAddToQueue();
            } else
            {
                Debug.Log("Game over!");
            }
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
                SpawnTetrominoAtTop(tetrominosToSpawn[0]);
                tetrominosToSpawn.Add(GenerateRandomTetromino());
                DisplayTetrisQueue();

            }
        }
    }
    private Tetris _activeTetris;
    public List<Tetris> tetrominosToSpawn = new List<Tetris>();
    [SerializeField] private int tetrominoSpawnOffset;
    public float cameraMoveSpeed;
    public static Tilemap _tileMap;
    [SerializeField] private TileBase tile;
    [Header("Queue values")]
    [SerializeField] float firstScale; [SerializeField] float secondScale, thirdScale;
    [SerializeField] Transform firstPos, secondPos, thirdPos;
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

    public void SelectThreeTetrominoToAddToQueue()
    {
        for (int i = 0; i < 3; i++)
        {
            tetrominosToSpawn.Add(GenerateRandomTetromino());
        }
        DisplayTetrisQueue();
    }
    public void DisplayTetrisQueue()
    {

        tetrominosToSpawn[0].transform.position = firstPos.position;
        tetrominosToSpawn[0].transform.localScale = Vector2.one *firstScale;
        tetrominosToSpawn[0].transform.parent = firstPos;

        tetrominosToSpawn[1].transform.position = secondPos.position;
        tetrominosToSpawn[1].transform.localScale = Vector2.one * secondScale;
        tetrominosToSpawn[1].transform.parent = secondPos;

        tetrominosToSpawn[2].transform.position = thirdPos.position;
        tetrominosToSpawn[2].transform.localScale = Vector2.one * thirdScale;
        tetrominosToSpawn[2].transform.parent = thirdPos;
    }
    public Tetris GenerateRandomTetromino()
    {

        int randomNum = Random.Range(0, tetrominoPrefabsList.Count);
        GameObject spawnedTetromino = Instantiate(tetrominoPrefabsList[randomNum], TetriiGroup.transform);
        return spawnedTetromino.GetComponent<Tetris>();
    }
    public void SpawnTetrominoAtTop(Tetris tetromino)
    {

        Vector3Int spawnPosition = new Vector3Int(-1, (int)Camera.main.ViewportToWorldPoint(Vector2.one).y + tetrominoSpawnOffset);
        tetromino.transform.parent = null;
        tetromino.transform.position = spawnPosition;
        tetromino.transform.localScale = Vector2.one;
        activeTetris = tetromino;
        activeTetris.EnableFalling();

        if (tetrominosToSpawn.Contains(activeTetris))
        {
            tetrominosToSpawn.Remove(activeTetris);
        }
        
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

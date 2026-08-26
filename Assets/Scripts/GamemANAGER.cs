using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GamemANAGER : MonoBehaviour 
{
    public static GamemANAGER instance;
    [SerializeField] private Tilemap _tilemap;
    [HideInInspector] public Dictionary<Vector3Int , Node> nodes = new Dictionary<Vector3Int , Node>();
    public bool gameInPlay = true;
    public List<GameObject> tetrominoPrefabsList = new List<GameObject>();
    public float timeToSpawnATetromino, currentTime, tetrominoFallingSpeed;
    public int score;
    [SerializeField] Vector3Int spawnPosition = new Vector3Int(-1,8,0);
    private GameObject TetriiGroup;
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _tilemap = FindAnyObjectByType<Tilemap>();
        foreach(Vector3Int pos in _tilemap.cellBounds.allPositionsWithin)
        {
            nodes[pos] = new Node(pos);
        }

        TetriiGroup = new GameObject("TetriiGroup");
    }

    private void Update()
    {
        if(gameInPlay) currentTime += Time.deltaTime;


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
        spawnedTetromino.GetComponent<Tetris>().EnableFalling();
    }

    public bool CheckIfTileIsAtLocation(Vector3 position)
    {
        return (_tilemap.HasTile(_tilemap.WorldToCell(position)));
    }
}

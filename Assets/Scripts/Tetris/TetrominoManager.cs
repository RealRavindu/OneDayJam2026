using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
//This class handles spawning tetrominos
public class TetrominoManager : MonoBehaviour
{
    public static TetrominoManager instance;
    public List<TetrominoData> Tetrominos = new List<TetrominoData>();
    public GameObject TetrominoPrefab;
    public static Tilemap tilemap;
    private void Start()
    {
        instance = this;
        tilemap = GameObject.FindAnyObjectByType<Tilemap>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse detected");
            SpawnRandomTetromino(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
    public void SpawnRandomTetromino(Vector2 position)
    {
        int randomNum = Random.Range(0, 7);
        SpawnTetromino(Tetrominos[randomNum], position);
    }
    void SpawnTetromino(TetrominoData data, Vector2 pos)
    {
        GameObject spawnedTetromino = Instantiate(TetrominoPrefab, transform);
        spawnedTetromino.GetComponent<Tetromino>().InstantiateTetromino(data);
        Vector3 spawnPos = new Vector2();
        if (data.shape == TetrominoShape.I || data.shape == TetrominoShape.O)
        {

            spawnPos = pos + (Vector2.one * -0.5f);
        } else
        {
            spawnPos = tilemap.WorldToCell(pos);
        }
        spawnedTetromino.transform.position = spawnPos;

        //temporary line for testing, later will handle setting active tetrominos a diff way maybe idk
        GameManager.instance.ActiveTetromino = spawnedTetromino.GetComponent<Tetromino>();
    }
}


public enum TetrominoShape //names of the 7 tetromino shapes
{
    I, O, L, J, T, Z, S
}

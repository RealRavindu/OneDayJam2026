using UnityEngine;

public class HazardSPawner : MonoBehaviour
{

    public Camera cam;
    public int gridWidth;
    public float tileSize;
    private float halfTileSize;

    public float baseSpawnRate;
    public AnimationCurve varianceCurve;
    public AnimationCurve intensityIncreaseCurve;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 camPos = Camera.main.transform.position;
        int camHeight = Mathf.RoundToInt(camPos.y);
    }
}

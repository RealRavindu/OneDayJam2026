using UnityEngine;
using System;
using System.Collections.Generic;

public class SRS : MonoBehaviour
{ 
    public TextAsset TextAsset;
    public Dictionary<int, Vector2Int> testSequences = new Dictionary<int, Vector2Int>();

    //Differentiate between I and O
    //Differentiate between the 4 states
    private void Start()
    {
        ReadCSVFile();
    }
    public void ReadCSVFile()
    {

        string[] data = TextAsset.text.Split(',');
        for(int i =0; i< data.Length; i+=9)
        {
            Debug.Log($"i: {i} | i in data: {int.Parse(data[i])}");
            testSequences.Add(int.Parse(data[i]), Vector2Int.zero);
            for(int j=i+1; j< i+1+data.Length/2; j+=8)
            {
                testSequences[int.Parse(data[i])] = new Vector2Int(int.Parse(data[j]), int.Parse(data[j + 1]));
                Debug.Log($"j vector: {new Vector2Int(int.Parse(data[j]), int.Parse(data[j + 1]))}");
            }
        }
        Debug.Log($"final dictionary keys (i) size: {testSequences.Keys.Count} |final dictionary values (j) size: {testSequences.Values.Count}");
    }
}


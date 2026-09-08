using UnityEngine;
using System;
using System.Collections.Generic;

public class SRS : MonoBehaviour
{
    public static SRS instance;
    public TextAsset TextAsset;
    public Dictionary<int, List<Vector2Int>> O_TestSequences = new Dictionary<int, List<Vector2Int>>();
    public Dictionary<int, List<Vector2Int>> I_TestSequences = new Dictionary<int, List<Vector2Int>>();

    //Differentiate between I and O
    //Differentiate between the 4 states
    private void Start()
    {
        instance = this;
        O_TestSequences = ReadCSVFile(0);
        I_TestSequences = ReadCSVFile(1);
    }
    public Dictionary<int, List<Vector2Int>> ReadCSVFile(int startNum)
    {
        Dictionary<int, List<Vector2Int>> testSequences = new Dictionary<int, List<Vector2Int>>();
        string[] data = TextAsset.text.Split(',','\n');
        for(int i =0 ; i< 4; i++)
        {
            testSequences.Add(i, new List<Vector2Int>());
            
            for(int j=i*2 +((data.Length / 2)*startNum); j< i+data.Length/2 +((data.Length / 2)*startNum); j+=8)
            {
                testSequences[i].Add(new Vector2Int(int.Parse(data[j]), int.Parse(data[j + 1])));
            }
        }
        return testSequences;
    }

    
}


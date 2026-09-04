using UnityEngine;
using System;

public class CSVReader : MonoBehaviour
{
    public TextAsset TextAsset;

    [System.Serializable]
    public class SRS
    {
        public Vector2Int[] SRSPositions = new Vector2Int[5];
    }
    //Differentiate between I and O
    //Differentiate between the 4 states
    public void ReadCSVFile()
    {
        //string[] data = TextAsset.text.Split()
        //DICTIONARY FOR SRS POSITIONS BECAUSE NEED TO GET KEY WHICH WILL BE THE ROTATION INDEX
    }
}

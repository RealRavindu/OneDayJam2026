using UnityEngine;
using System;
using System.Collections.Generic;

public class SRS : MonoBehaviour
{
    //SRS stands for Super Rotation System
    //It is a standardized system most Tetris games use to facilitate clockwise rotation checks.
    //By clockwise rotation checks I mean that when you try and rotate a tetromino, you must first check if it is possible to rotate without collisions.
    //If there is a collision, the SRS then tries to offset the tetromino by a little bit and check if it's possible for the tetromino to then sit in that new position.
    //This class reads a CSV file and stores test sequences that are used by the tetrominos to check if their rotations are possible.
    //A test sequence is a series of 5 Vector2 offsets that attempt to shift a tetromino if a rotation isn't possible due to collision. The first offset in every sequence is (0,0)
    //to test the original position it's supposed to be in. If that fails it shifts a little bit like to (1,0) for example and checks if it's possible to be in that position without
    //colliding with anything. It does this 5 times and if all tests fail, only then will it fail the rotation and stay exactly as is.
    //Each Tetromino has an int called 'rotation index'. This index is the key used by the dictionaries. A tetromino when rotating has 4 directions it can face.
    //O is the original direction it faces, then (going clockwise) is R (Right), 2 (2 from right, or 2 from left). then L (left).
    //      O
    //  L       R
    //      2
    //Is a good way to imagine it. So a rotation index of 0 would mean that the tetromino is going from L > 0 (rotating clockwise from left to original). A rotation index
    //of 1 would be 0 > R. 2 is R > 2. 3 is 2 > L and then it loops from there. Since currently in this game's scope we are only doing clockwise rotation there are only 4 rotation indices.
    //However, if counter-clockwise rotation is introduced we will then need 4 additional rotation indices for counter-clockwise changes (L > 2, 2 > R, R > 0, 0 > L) and we'll need to
    //edit the CSV file to have 4 additional rows with their respective SRS standardized offsets.


    //since 'I' shaped tetrominos and every other tetromino use 2 seperate sets of sequences they are divided into 2 dictionarys I(I line)_TestSequences and O(Other)_TestSequences
    public static SRS instance;
    public TextAsset TextAsset;
    public Dictionary<int, List<Vector2Int>> O_TestSequences = new Dictionary<int, List<Vector2Int>>();
    public Dictionary<int, List<Vector2Int>> I_TestSequences = new Dictionary<int, List<Vector2Int>>();

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


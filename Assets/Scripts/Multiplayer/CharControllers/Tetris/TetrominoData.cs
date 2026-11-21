using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Netcode;
//This class contains data on the 7 different tetrominos i.e. where each block in a tetromino is placed.
[System.Serializable]
public class TetrominoData
{
    public TetrominoShape shape;
    public Vector2[] blockPositions = new Vector2[4];
}
/*
public class NetworkableTetrominoData : INetworkSerializable
{
    public TetrominoShape shape;
    public Vector2 block1, block2, block3, block4;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref shape);
        serializer.SerializeValue(ref block1);
        serializer.SerializeValue(ref block2);
        serializer.SerializeValue(ref block3);
        serializer.SerializeValue(ref block4);
    }
}*/
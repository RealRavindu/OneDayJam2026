using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Netcode;
//This class contains data on the 7 different tetrominos i.e. where each block in a tetromino is placed.
[System.Serializable]
public class TetrominoData: INetworkSerializable
{
    public TetrominoShape shape;
    public Vector2[] blockPositions = new Vector2[4];

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref shape);
        serializer.SerializeValue(ref blockPositions);
    }
}

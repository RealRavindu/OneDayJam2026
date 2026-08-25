using UnityEngine;

public class GamemANAGER : MonoBehaviour 
{
    public static GamemANAGER instance;

    private void Awake()
    {
        instance = this;
    }
}

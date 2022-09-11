using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Data
{
    public string name;
    public float Time;
}

public class LeaderBoardData : MonoBehaviour
{
    public static LeaderBoardData instance;
    [SerializeField]
    List<Data> leaderBoard;

    private void Awake()
    {
        instance = this;
    }

    public void SetData(string name, float clocktime)
    {
        leaderBoard.Find((x) => x.name == name).Time = clocktime;
    }
    
    public float GetData(string name)
    {
        return leaderBoard.Find((x) => x.name == name).Time;
    }
}

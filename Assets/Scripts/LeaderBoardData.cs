using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Data
{
    public string name;
    public float Time;

    public Data(string playerName, float gametime)
    {
        name = playerName;
        Time = gametime;
    }
}

public class LeaderBoardData : MonoBehaviour
{
    public static LeaderBoardData instance;
    [SerializeField]
    List<Data> leaderBoard;
    public GameObject dataSet, dataSetParent;

    private void OnEnable()
    {
        EventManager.instance.onFail += CreateRows;
        EventManager.instance.onWin += CreateRows;

    }

    private void OnDisable()
    {
        EventManager.instance.onFail -= CreateRows;
        EventManager.instance.onWin -= CreateRows;
    }

    private void Awake()
    {
        instance = this;
    }

    public void SetData(string name, float clocktime)
    {
        Debug.Log("SetDAta");
        //leaderBoard.Find((x) => x.name == name).Time = clocktime;
        leaderBoard.Add(new Data(name, clocktime));
    }
    
    public void CreateRows()
    {
        int i = 0;
        foreach(Data x in leaderBoard)
        {
            GameObject data = Instantiate(dataSet, dataSetParent.transform);
            data.GetComponent<RectTransform>().localPosition = new Vector3(-8.9f, i * 129.11f, 0);
            data.GetComponent<RectTransform>().rotation = Quaternion.identity;
            data.GetComponent<DataRef>().name.text = x.name;

            int minutes = (int)x.Time / 60;
            int seconds = (int)x.Time % 60;

            data.GetComponent<DataRef>().time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            i++;
        }
        GameManager.instance.ShowLeaderBoard();
    }

    public float GetData(string name)
    {
        return leaderBoard.Find((x) => x.name == name).Time;
    }
}

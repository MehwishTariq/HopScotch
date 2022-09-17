using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Data
{
    public string name;
    public float Time;
    public float Score;
    public float maxScore;

    public Data(string playerName, float gametime, float _maxScore)
    {
        name = playerName;
        Time = gametime;
        maxScore = _maxScore;
    }
}

public class LeaderBoardData : MonoBehaviour
{
    public static LeaderBoardData instance;
    [SerializeField]
    List<Data> leaderBoard;
    public GameObject dataSet, dataSetParent;

    
    [SerializeField]
    float minTime;

    

    [SerializeField]
    float currentScore = 0;

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

    public float Score(float playerTime, float max_Score)
    {
        float x = minTime / playerTime;
        currentScore = max_Score * x;
        return currentScore;
    }

    public void SetData(string name, float clocktime, float _maxScore)
    {
        Debug.Log("SetDAta");
        if (leaderBoard.Contains(new Data(name, 0, _maxScore)))
        {
            leaderBoard.Find((x) => x.name == name).Time = clocktime;
            leaderBoard.Find((x) => x.name == name).maxScore = _maxScore;
        }
        else
            leaderBoard.Add(new Data(name, clocktime, _maxScore));
    }
    
    public void CreateRows()
    {
        int i = 0;
        foreach(Data x in leaderBoard)
        {
            GameObject data = Instantiate(dataSet, dataSetParent.transform);
            data.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, (i+1) * 105f, 0);
            data.GetComponent<RectTransform>().rotation = Quaternion.identity;
            data.GetComponent<DataRef>().Name.text = x.name;
            data.GetComponent<DataRef>().score.text = Score(x.Time, x.maxScore).ToString();
            

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

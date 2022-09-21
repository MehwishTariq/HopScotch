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

    public Data(string playerName, float gametime, float _maxScore, float _score)
    {
        name = playerName;
        Time = gametime;
        maxScore = _maxScore;
        Score = _score;
    }
}

public class LeaderBoardData : MonoBehaviour
{
    public static LeaderBoardData instance;
    [SerializeField]
    List<Data> leaderBoard;
    public GameObject dataSet;
    
    [SerializeField]
    float minTime;


    int i = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        EventManager.instance.onAddData += ResetInt;
        
    }

    private void OnDisable()
    {
        EventManager.instance.onAddData -= ResetInt;
    }

    void ResetInt()
    {
        i = 1;
    }
    public int Score(float playerTime, float max_Score)
    {
        float x = minTime / playerTime;
        return (int)(max_Score * x);
    }
    
    public void SetData(string name, float clocktime, float _maxScore)
    {
        Debug.Log("SetDAta" + PlayerPrefs.GetInt(name, 0));
        int notFound = 0;
        foreach (Data x in leaderBoard)
        {
            if (x.name == name)
            {
                x.Time = clocktime;
                x.maxScore = _maxScore;
                x.Score = PlayerPrefs.GetInt(name);
                break;
            }
            else
                notFound++;
        }
        Debug.Log("notFound" + notFound);

        if (notFound == leaderBoard.Count)
        {
            if(GameManager.instance.gameFailed && name == "Player")
                leaderBoard.Add(new Data(name, clocktime, _maxScore, Score(clocktime, 0)));
            else if(GameManager.instance.gameWon && name != "Player")
                leaderBoard.Add(new Data(name, clocktime, _maxScore, Score(clocktime, 0)));
            else
                leaderBoard.Add(new Data(name, clocktime, _maxScore, Score(clocktime, _maxScore)));
        }
        InstantiateDataSets(name, clocktime, _maxScore);
       
        i--;
        if (GameManager.instance.gameFailed || GameManager.instance.gameWon)
        {
            GameManager.instance.ShowLeaderBoard();
            GameManager.instance.gameStarted = false;
        }
    }
    
    public void InstantiateDataSets(string name, float clocktime, float _maxScore)
    {
        GameObject data = Instantiate(dataSet, GameManager.instance.leaderBoard.transform);
        data.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, i * 105f, 0);
        data.GetComponent<RectTransform>().rotation = Quaternion.identity;
        data.GetComponent<DataRef>().Name.text = name;

        if (GameManager.instance.gameFailed && name == "Player")
        {
            PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + Score(clocktime, 0));
        }
        else if (GameManager.instance.gameWon && name != "Player")
        {
            PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + Score(clocktime, 0));
        }
        else
        {
            PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + Score(clocktime, _maxScore));
        }
        data.GetComponent<DataRef>().score.text = PlayerPrefs.GetInt(name).ToString();
        //int minutes = (int)clocktime / 60;
        //int seconds = (int)clocktime % 60;

        //data.GetComponent<DataRef>().time.text = string.Format("{0:00}:{1:00}", minutes, seconds);


    }

    //public void CreateRows()
    //{
    //    int i = 0;
    //    foreach(Data x in leaderBoard)
    //    {
    //        GameObject data = Instantiate(dataSet, dataSetParent.transform);
    //        data.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, (i+1) * 105f, 0);
    //        data.GetComponent<RectTransform>().rotation = Quaternion.identity;
    //        data.GetComponent<DataRef>().Name.text = x.name;
    //        data.GetComponent<DataRef>().score.text = Score(x.Time, x.maxScore).ToString();


    //        int minutes = (int)x.Time / 60;
    //        int seconds = (int)x.Time % 60;

    //        data.GetComponent<DataRef>().time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    //        i++;
    //    }
    //    GameManager.instance.ShowLeaderBoard();
    //}

    public float GetData(string name)
    {
        return leaderBoard.Find((x) => x.name == name).Time;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Data
{
    public string name;
    public float Time;
    public int Score;
    public float maxScore;

    public Data(string playerName, float gametime, float _maxScore, int _score)
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
    [SerializeField]
    TimeManager playerData;

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

    public int dataForTwo = 0;
    public void SetData(string name, float clocktime, float _maxScore)
    {
        int notFound = 0;
        foreach (Data x in leaderBoard)
        {
            if (x.name == name)
            {
                x.Time = clocktime;
                x.maxScore = _maxScore;
                x.Score = PlayerPrefs.GetInt(name,0);
                break;
            }
            else
                notFound++;
        }
        
        if (notFound == leaderBoard.Count)
        {
            if (GameManager.instance.gameFailed && name == playerData.PlayerName)
            {
                leaderBoard.Add(new Data(name, clocktime, _maxScore, PlayerPrefs.GetInt(name, 0) + Score(clocktime, 0)));
                PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + Score(clocktime, 0));
            }
            else if (GameManager.instance.gameWon && name != playerData.PlayerName)
            { 
                leaderBoard.Add(new Data(name, clocktime, _maxScore, PlayerPrefs.GetInt(name, 0) + Score(clocktime, 0)));
                PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + Score(clocktime, 0));
            }
            else
            { 
                leaderBoard.Add(new Data(name, clocktime, _maxScore, PlayerPrefs.GetInt(name, 0) + Score(clocktime, _maxScore)));
                PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + Score(clocktime, _maxScore));
            }
        }

       
        dataForTwo++;


        if ((GameManager.instance.gameFailed || GameManager.instance.gameWon) && dataForTwo >= 5)
        {
            SortList();
            InstantiateDataSets();

            GameManager.instance.ShowResultScreen();
            GameManager.instance.gameStarted = false;
            dataForTwo = 0;
        }
    }
    
    void SortList()
    {
        leaderBoard.Sort(delegate (Data a, Data b)
        {
            return a.Score
            .CompareTo(
              b.Score);
        });
        leaderBoard.Reverse();
    }

    public void InstantiateDataSets()
    {
        int i = 1, j = 1;
        foreach (Data x in leaderBoard)
        {
            GameObject data = Instantiate(dataSet, GameManager.instance.content.transform);
            //data.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -100 - (185 * i)/*(i * -186) - 285f*/, 0);
            //data.GetComponent<RectTransform>().rotation = Quaternion.identity;
            data.GetComponent<DataRef>().Name.text = x.name;
            data.GetComponent<DataRef>().rank.text = j.ToString()+".";
            
            data.GetComponent<DataRef>().score.text = PlayerPrefs.GetInt(x.name).ToString();

            j++;
            i++;

        }

    }

    public float GetData(string name)
    {
        return leaderBoard.Find((x) => x.name == name).Time;
    }
}

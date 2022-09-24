using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public string PlayerName;
    public Text time, playerName;
    public float foulTimeAdd;
    public float maxScore;

    float clockTime = 0;
    [SerializeField]
    bool allowTimer;
    

    private void Start()
    {
        playerName.text = PlayerName;
        EventManager.instance.onStart += StartTimer;
        EventManager.instance.onFail += SetTimeinBoard;
        EventManager.instance.onWin += SetTimeinBoard;

    }

    private void OnDisable()
    {
        EventManager.instance.onStart -= StartTimer;
        EventManager.instance.onFail -= SetTimeinBoard;
        EventManager.instance.onWin -= SetTimeinBoard;
    }

    public void StartTimer()
    {
       
        allowTimer = true;
       
    }
    // Update is called once per frame
    void Update()
    {
        if (allowTimer)
            GameTimer();
    }

    public void SetTimeinBoard()
    {
        allowTimer = false;
        LeaderBoardData.instance.SetData(PlayerName, clockTime, maxScore);
        
    }


    public void AddTimeOnFoul()
    {
        
        clockTime += foulTimeAdd;
        time.GetComponent<RectTransform>().DOScale(1.5f, 0.3f).OnComplete(() => {
            time.GetComponent<RectTransform>().DOScale(1f, 0.3f);
        });
        GameManager.instance.wrongJump = false;
    }

    void GameTimer()
    {
        clockTime += Time.deltaTime;

        int minutes = (int)clockTime / 60;
        int seconds = (int)clockTime % 60;

        time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

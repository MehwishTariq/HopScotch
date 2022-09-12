using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public string PlayerName;
    public Text time;
    public float foulTimeAdd;

    float clockTime = 0;
    bool allowTimer = false;
    

    private void OnEnable()
    {
        EventManager.instance.onStart += StartTimer;
        EventManager.instance.onFoul += AddTimeOnFoul;
        EventManager.instance.onFail += SetTimeinBoard;
        EventManager.instance.onWin += SetTimeinBoard;

    }

    private void OnDisable()
    {
        EventManager.instance.onStart -= StartTimer;
        EventManager.instance.onFoul -= AddTimeOnFoul;
        EventManager.instance.onFail -= SetTimeinBoard;
        EventManager.instance.onWin -= SetTimeinBoard;
    }

    void StartTimer()
    {
        Debug.Log("startTimer");
        allowTimer = !allowTimer;
       
    }
    // Update is called once per frame
    void Update()
    {
        if (allowTimer)
            GameTimer();
    }

    public void SetTimeinBoard()
    {
        StartTimer();
        LeaderBoardData.instance.SetData(PlayerName, clockTime);
    }

    public void AddTimeOnFoul()
    {
        clockTime += foulTimeAdd;
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
